using System.Collections.ObjectModel;
using Budget.Command;
using Budget.Data;
using Budget.Models;

namespace Budget.ViewModels
{
    public class BudgetVM : ViewModelBase
    {
        private ObservableCollection<MonthVM> months = new();
        public ObservableCollection<MonthVM> Months
        {
            get
            {
                months.Clear();
                var allMonths = BudgetRepository.GetAllMonths();
                foreach (var time in allMonths)
                {
                    months.Add(new MonthVM(time));
                }
                return months;
            }
            set
            {
                months = value;
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
                if (selectedMonth != null) { BudgetRepository.UpdateMonth(selectedMonth); }
                RaisePropertyChanged();
                //ListAllTransactions();
                DeleteMonthCommand.RaiseCanExecuteChanged();
            }
        }
        private MoneyVM? selectedTrans;
        public MoneyVM? SelectedTrans
        {
            get { return selectedTrans; }
            set
            {
                if (selectedTrans != null) { BudgetRepository.UpdateMoneyTransaction(selectedTrans); }
                selectedTrans = value;
                RaisePropertyChanged();
                DeleteTransactionCommand.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand AddMonthCommand { get; }
        public DelegateCommand DeleteMonthCommand { get; }
        public DelegateCommand AddTransactionCommand { get; }
        public DelegateCommand DeleteTransactionCommand { get; }
        private BudgetRepository BudgetRepository { get; set; }
        public BudgetVM(ApplicationDbContext applicationDbContext) //CONSTRUCTOR
        {
            BudgetRepository = new BudgetRepository(applicationDbContext);

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
                BudgetRepository.DeleteMonth(SelectedMonth); //might be the double-layer here causing the problem
                Months.Remove(SelectedMonth); //removes from VM
                RaisePropertyChanged();
                SelectedMonth = null; //will update things on its own
            }
        }
        private bool CanDeleteMonth(object? parameter) => SelectedMonth is not null;
        private void AddMonth(object? parameter)
        {
            var previousMonth = BudgetRepository.GetLastMonth();
            MonthNameEnum monthName;
            if (previousMonth == null) { monthName = 0; }
            else if (previousMonth.Name == MonthNameEnum.December) { monthName = MonthNameEnum.January; }
            else { monthName = previousMonth.Name + 1; }
            Month month = new() { Name = monthName }; //new month is next month
            ///month = BudgetRepository.GiveMonthAnId(month);
            BudgetRepository.CreateMonth(month);
            months.Clear();
            var allMonths = BudgetRepository.GetAllMonths();
            foreach (var time in allMonths)
            {
                var temp = new MonthVM(time);
                months.Add(temp);
                if (time.Id == month.Id) { SelectedMonth = temp; }
            }
        }

        private void DeleteTransaction(object? parameter)
        {
            if (SelectedTrans is not null)
            {
                SelectedMonth.MoneyTrans.Remove(SelectedTrans);
                BudgetRepository.DeleteMoneyTransaction(SelectedTrans);
                SelectedTrans = null;
            }
        }
        private bool CanDeleteTransaction(object? parameter) => SelectedTrans is not null;
        private void AddTransaction(object? parameter) //CAN'T DO THIS -> lots of errors
        {
            MoneyTransaction trans = new();
            //trans = BudgetRepository.GiveMoneyAnId(trans); //to stop it from giving everything ID==0
            BudgetRepository.GiveMoneyAMonth(trans, SelectedMonth);
            BudgetRepository.CreateMoneyTransaction(trans);
            var transVM = new MoneyVM(trans);
            SelectedTrans = transVM;
        }

        //READ -> calculations
        private ObservableCollection<MoneyVM> ListAllTransactions()
        {
            if (SelectedMonth == null)
            {
                return null;
            }
            else
            {
                SelectedMonth.MoneyTrans.Clear();
                SelectedMonth.MoneyTrans = BudgetRepository.GetMonthlyTransactions(SelectedMonth);
                return SelectedMonth.MoneyTrans;
            }
        }
    }
}
