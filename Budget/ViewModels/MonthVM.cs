using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Command;
using Budget.Models;

namespace Budget.ViewModels
{
    public class MonthVM : ViewModelBase
    {
        private ObservableCollection<MoneyVM> moneyTrans = new();

        public ObservableCollection<MoneyVM> MoneyTrans
        {
            get { return moneyTrans; }
            set
            {
                moneyTrans = value;
                RaisePropertyChanged();
            }
        }

        //private MoneyVM? selectedTrans;
        //public MoneyVM? SelectedTrans
        //{
        //    get { return selectedTrans; }
        //    set
        //    {
        //        selectedTrans = value;
        //        RaisePropertyChanged();
        //        DeleteCommand.RaiseCanExecuteChanged();
        //    }
        //}

        private readonly Month month;

        public string Name
        {
            get { return month.Name; }
            set
            {
                month.Name = value;
                RaisePropertyChanged();
            }
        }

        //public DelegateCommand AddCommand { get; }
        //public DelegateCommand DeleteCommand { get; }
        public MonthVM(Month month) //CONSTRUCTOR
        {
            this.month = month;
            //AddCommand = new DelegateCommand(AddTrans);
            //DeleteCommand = new DelegateCommand(DeleteTrans, CanDelete);
        }

        //private void DeleteTrans(object? parameter)
        //{
        //    if (SelectedTrans is not null)
        //    {
        //        MoneyTrans.Remove(SelectedTrans);
        //        SelectedTrans = null;
        //    }
        //}
        //private bool CanDelete(object? parameter) => SelectedTrans is not null;
        //private void AddTrans(object? parameter)
        //{
        //    MoneyTransaction mTrans = new();
        //    var mTransVM = new MoneyVM(mTrans);
        //    MoneyTrans.Add(mTransVM);
        //    SelectedTrans = mTransVM;
        //}
    }
}
