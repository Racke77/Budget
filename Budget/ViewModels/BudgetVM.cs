using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Calculations;
using Budget.Command;
using Budget.Models;

namespace Budget.ViewModels
{
    public class BudgetVM : ViewModelBase
    {
        private ObservableCollection<MonthVM> months = new();
        public ObservableCollection<MonthVM> Months
        {
            get { return months; }
            set
            {
                months = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<MoneyVM> adjustedTransactions = new();
        public ObservableCollection<MoneyVM> AdjustedTransactions
        {
            get
            {
                return adjustedTransactions;
            }
            set
            {
                ListAllTransactions();
                //adjustedTransactions = value;
                RaisePropertyChanged();
            }
        }

        private MonthVM? selectedMonth;
        public MonthVM? SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                RaisePropertyChanged();
                ListAllTransactions();
                DeleteMonthCommand.RaiseCanExecuteChanged();
            }
        }
        private MoneyVM? selectedTrans;
        public MoneyVM? SelectedTrans
        {
            get { return selectedTrans; }
            set
            {
                selectedTrans = value;
                RaisePropertyChanged();
                DeleteTransactionCommand.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand AddMonthCommand { get; }
        public DelegateCommand DeleteMonthCommand { get; }
        public DelegateCommand AddTransactionCommand { get; }
        public DelegateCommand DeleteTransactionCommand { get; }
        public BudgetVM() //CONSTRUCTOR
        {
            //SEEDING
            Months.Add(new MonthVM(new Month() { Name = "Test 1" }));
            Months.Add(new MonthVM(new Month() { Name = "Test 2" }));

            foreach (MonthVM month in Months)
            {
                month.MoneyTrans.Add(new MoneyVM(new MoneyTransaction() { Money = 1000, Name = "Test money" }));
                month.MoneyTrans.Add(new MoneyVM(new MoneyTransaction() { Money = -1000, Name = "Test money" }));
            }

            //DELEGATE COMMANDS
            AddMonthCommand = new DelegateCommand(AddMonth);
            DeleteMonthCommand = new DelegateCommand(DeleteMonth, CanDeleteMonth);
            AddTransactionCommand = new DelegateCommand(AddTransaction);
            DeleteTransactionCommand = new DelegateCommand(DeleteTransaction, CanDeleteTransaction);
        }

        private void DeleteMonth(object? parameter)
        {
            if (SelectedMonth is not null)
            {
                Months.Remove(SelectedMonth);
                SelectedMonth = null;
            }
        }
        private bool CanDeleteMonth(object? parameter) => SelectedMonth is not null;
        private void AddMonth(object? parameter)
        {
            Month month = new();
            var monthVM = new MonthVM(month);
            Months.Add(monthVM);
            SelectedMonth = monthVM;
        }

        private void DeleteTransaction(object? parameter)
        {
            if (SelectedTrans is not null)
            {
                SelectedMonth.MoneyTrans.Remove(SelectedTrans);
                SelectedTrans = null;
            }
        }
        private bool CanDeleteTransaction(object? parameter) => SelectedTrans is not null;
        private void AddTransaction(object? parameter)
        {
            MoneyTransaction trans = new()
            {
                Id = SelectedMonth.MoneyTrans.Count
            }; //to stop it from giving everything ID==0
            var transVM = new MoneyVM(trans);
            SelectedMonth.MoneyTrans.Add(transVM);
            SelectedTrans = transVM;
        }

        //READ -> calculations
        private ObservableCollection<MoneyVM> ListAllTransactions()
        {
            if (SelectedMonth == null)
            {
                return adjustedTransactions;
            }
            else
            {
                return SelectedMonth.MoneyTrans;
                adjustedTransactions.Clear();
                foreach (var money in SelectedMonth.MoneyTrans)
                {
                //    if (money.IsThisIncome == true) //income
                //    {
                //        if (money.IsThisSalary == true) //salary
                //        {
                //            var salaryCalculator = new CalculateSalary(money.Money); //send in base-money
                //            if (money.Money == money.CalculatedValue) //value has not been calculated
                //            {
                //                money.CalculatedValue = salaryCalculator.SalaryCalc(money); //update with calculated value
                //            }
                //            else { } //salary has already been adjusted -> don't touch
                //        }
                //    }
                //    else //expense
                //    {
                //        if (money.CalculatedValue > 0) { money.CalculatedValue = money.CalculatedValue * -1; } //turn into negative numbers if not already done
                //    }
                    adjustedTransactions.Add(money);
                }

                //var positiveMoney = SelectedMonth.MoneyTrans.Where(x => x.IsThisIncome).ToList();
                //var negativeMoney = SelectedMonth.MoneyTrans.Where(x => x.IsThisIncome == false).ToList();

                //foreach (var sMoney in positiveMoney)
                //{
                //    if (sMoney.IsThisSalary==true)
                //    {
                //        //find the original one
                //        var original = SelectedMonth.MoneyTrans.Where(x=>x.Id==sMoney.Id).FirstOrDefault();
                //        //compare ORIGINAL to CURRENT


                //        var salaryCalculator = new CalculateSalary(original); //send in ORIGINAL moneyVM
                //        if (salaryCalculator.HasSalaryBeenCalculated(sMoney) == false)//test method for bool
                //        {
                //            sMoney.Money = salaryCalculator.SalaryCalc(sMoney);
                //        }
                //        else { } //salary has already been adjusted -> don't touch
                //    }
                //}
                //foreach (var pMoney in positiveMoney)
                //{
                //    adjustedTransactions.Add(pMoney);
                //}
                //foreach (var nMoney in negativeMoney)
                //{
                //    if (nMoney.Money > 0) { nMoney.Money = nMoney.Money * -1; } //turn into negative numbers if not already done
                //    adjustedTransactions.Add(nMoney);
                //}
                return adjustedTransactions;
            }
        }
    }
}
