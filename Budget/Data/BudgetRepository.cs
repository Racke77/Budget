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
            var temp = GetLastMonth();
            foreach(var item in temp.ListMoney)
            {
                if (item.Reocurring==true)
                {
                    var tempItem = CopyMoneyTransaction(item);
                    month.ListMoney.Add(tempItem);
                }
            }
            return month;
        }

        private MoneyTransaction CopyMoneyTransaction(MoneyTransaction oldMoney)
        {
            var newMoney = new MoneyTransaction();
            newMoney = GiveMoneyAnId(newMoney);
            newMoney.Name = oldMoney.Name;
            newMoney.Money = oldMoney.Money;
            newMoney.IsThisIncome = oldMoney.IsThisIncome;
            newMoney.IsThisSalary = oldMoney.IsThisSalary;
            newMoney.Reocurring = oldMoney.Reocurring;
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
                .Include(z=>z.ListMoney)
                .LastOrDefault();
        }

        //UPDATE
        public Month GiveMonthAnId(Month month)
        {
            month.Id = applicationDbContext.Months.OrderBy(z => z.Id).LastOrDefault().Id+1; //stop whining about the ID you whimp
            return month;
        }
        public MoneyTransaction GiveMoneyAnId(MoneyTransaction money)
        {
            money.Id = applicationDbContext.MoneyTransactions.OrderBy(z=>z.Id).LastOrDefault().Id + 1;
            return money;
        }
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
                .Include(m=>m.ListMoney)
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

            applicationDbContext.MoneyTransactions.Update(ConvertMoneyVMToMoney(money)); //overwrite due to identical ID
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
            temp.IsThisSalary = moneyVM.IsThisSalary;
            temp.Reocurring = moneyVM.Reocurring;
            temp.CalculateValue();
            temp.SickDays = moneyVM.SickDays;
            return temp;
        }

        //DELETE
        public void DeleteMonth(MonthVM month) //error now
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
