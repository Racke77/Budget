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
            get { return months; }
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
        private BudgetRepository BudgetRepository { get; set; }
        public BudgetVM(ApplicationDbContext applicationDbContext) //CONSTRUCTOR
        {
            //SEEDING
            Months.Add(new MonthVM(new Month() { Name = "Test 1" }));
            Months.Add(new MonthVM(new Month() { Name = "Test 2" }));

            foreach (MonthVM month in Months)
            {
                month.MoneyTrans.Add(new MoneyVM(new MoneyTransaction() { Money = 1000, Name = "Test money" }));
                month.MoneyTrans.Add(new MoneyVM(new MoneyTransaction() { Money = -1000, Name = "Test money" }));
            }

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
                Months.Remove(SelectedMonth);
                SelectedMonth = null;
            }
        }
        private bool CanDeleteMonth(object? parameter) => SelectedMonth is not null;
        private void AddMonth(object? parameter)
        {
            Month month = new();
            BudgetRepository.CreateMonth(month);
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
                Id = SelectedMonth.MoneyTrans.Count //to stop it from giving everything ID==0
            }; 
            var transVM = new MoneyVM(trans);
            SelectedMonth.MoneyTrans.Add(transVM);
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
                return SelectedMonth.MoneyTrans;
            }
        }
    }
}
