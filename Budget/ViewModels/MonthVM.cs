using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Calculations;
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

        private readonly Month month;

        public MonthNameEnum Name
        {
            get { return month.Name; }
            set
            {
                month.Name = value;
                RaisePropertyChanged();
            }
        }
        public int Id
        {
            get { return month.Id; }
            set
            {
                month.Id = value;
                RaisePropertyChanged();
            }
        }
        private float calcFullNumber = new();
        public float CalcFullNumber
        {
            get {
                var test = new MonthlyCalc(this);
                calcFullNumber = test.MonthlyPlusMinus();
                return calcFullNumber; }
            set
            {
                var test = new MonthlyCalc(this);
                calcFullNumber = test.MonthlyPlusMinus();
            }
        }

        public MonthVM(Month month) //CONSTRUCTOR
        {
            this.month = month;
        }
    }
}
