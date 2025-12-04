using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Models;
using Budget.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Budget.Data
{
    public class BudgetRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public BudgetRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        //CREATE
        public void CreateMonth(Month month)
        {
            month = RecreateReocurringMoney(month);
            applicationDbContext.Months.Add(month);
            applicationDbContext.SaveChanges();
        }
        private Month RecreateReocurringMoney(Month month) //don't do anything with VM -> SelectedMonth updates that
        {
            var prev = GetLastMonth(); //previous month
            if (prev != null)
            {
                foreach (var item in prev.ListMoney)
                {
                    if (item.ReocurringMonth == true) //if it will reoccur monthly make a copy of it
                    {
                        var tempItem = CopyMoneyTransaction(item);
                        month.ListMoney.Add(tempItem);
                    }
                }
                var temp2 = GetLastEnumMonth(month); //last year
                if (temp2 != null)
                {
                    foreach (var item in temp2.ListMoney)
                    {
                        if (item.ReocurringYear == true) //if it will reoccur yearly make a copy of it
                        {
                            var tempItem = CopyMoneyTransaction(item);
                            month.ListMoney.Add(tempItem);
                        }
                    }
                }
            }
            return month;
        }


        private MoneyTransaction CopyMoneyTransaction(MoneyTransaction oldMoney)
        {
            var newMoney = new MoneyTransaction();
            newMoney.Name = oldMoney.Name;
            newMoney.Money = oldMoney.Money;
            newMoney.IsThisIncome = oldMoney.IsThisIncome;
            newMoney.Category = oldMoney.Category;
            newMoney.ReocurringMonth = oldMoney.ReocurringMonth;
            newMoney.ReocurringYear = oldMoney.ReocurringYear;
            newMoney.SickDays = 0;
            newMoney.CalculateValue();
            return newMoney;
        }

        public void CreateMoneyTransaction(MoneyTransaction money)
        {
            applicationDbContext.MoneyTransactions.Add(money);
            applicationDbContext.SaveChanges();
        }

        //READ
        public List<Month> GetAllMonths()
        {
            return applicationDbContext.Months
                .OrderByDescending(m=>m.Id)
                .ToList();
        }
        public ObservableCollection<MoneyVM> GetMonthlyTransactions(MonthVM month)
        {
            var temp = applicationDbContext.Months
                .Where(m => m.Id == month.Id)
                .Include(m => m.ListMoney)
                .FirstOrDefault();
            temp.ListMoney.DistinctBy(x => x.Id); //making sure no doubles get in
            var moneyTemp = new ObservableCollection<MoneyVM>();
            foreach (var item in temp.ListMoney)
            {
                moneyTemp.Add(new MoneyVM(item));
            }

            return moneyTemp;
        }
        public Month GetLastMonth()
        {
            return applicationDbContext.Months
                .OrderBy(z => z.Id)
                .Include(z => z.ListMoney)
                .LastOrDefault();
        }
        private Month GetLastEnumMonth(Month month)
        {
            return applicationDbContext.Months
                .Where(z => z.Name == month.Name) //only check accurate enum
                .OrderBy(z => z.Id)
                .Include(z => z.ListMoney)
                .LastOrDefault();
        }

        //UPDATE
        public void GiveMoneyAMonth(MoneyTransaction money, MonthVM monthVM)
        {
            var temp = applicationDbContext.Months
                .Where(m => m.Id == monthVM.Id)
                .Include(m => m.ListMoney)
                .FirstOrDefault();
            temp.ListMoney.Add(money);
        }
        public void UpdateMonth(MonthVM month)
        {
            var temp = applicationDbContext.Months
                .Where(m => m.Id == month.Id)
                .Include(m => m.ListMoney)
                .FirstOrDefault();
            temp.Name = month.Name;
            //any changes to MONEY will go through database
            //trust in the list that's pulled from database
            month.MoneyTrans.Clear();
            foreach (var money in temp.ListMoney)
            {
                month.MoneyTrans.Add(new MoneyVM(money));
            }

            applicationDbContext.Months.Update(temp);
            applicationDbContext.SaveChanges();
        }
        public void UpdateMoneyTransaction(MoneyVM money)
        {
            applicationDbContext.MoneyTransactions.Update(ConvertMoneyVMToMoney(money));
            applicationDbContext.SaveChanges();
        }
        private MoneyTransaction ConvertMoneyVMToMoney(MoneyVM moneyVM)
        {
            var temp = applicationDbContext.MoneyTransactions
                .Where(m => m.Id == moneyVM.Id)
                .FirstOrDefault();
            temp.Name = moneyVM.Name;
            temp.Money = moneyVM.Money;
            temp.IsThisIncome = moneyVM.IsThisIncome;
            temp.Category = moneyVM.Category;
            temp.ReocurringMonth = moneyVM.ReocurringMonth;
            temp.ReocurringYear = moneyVM.ReocurringYear;
            temp.CalculateValue();
            temp.SickDays = moneyVM.SickDays;
            return temp;
        }

        //DELETE
        public void DeleteMonth(MonthVM month)
        {
            applicationDbContext.Months.Remove(
                applicationDbContext.Months
                    .Where(m => m.Id == month.Id)
                    .FirstOrDefault());
            applicationDbContext.SaveChanges();
        }
        public void DeleteMoneyTransaction(MoneyVM money)
        {
            applicationDbContext.MoneyTransactions.Remove(
                    applicationDbContext.MoneyTransactions
                        .Where(m => m.Id == money.Id)
                        .FirstOrDefault());
            applicationDbContext.SaveChanges();
        }
    }
}
