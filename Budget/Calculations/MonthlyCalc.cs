using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.ViewModels;

namespace Budget.Calculations
{
    public class MonthlyCalc
    {
        public MonthVM MonthVM { get; set; }
        public MonthlyCalc(MonthVM monthVM)
        {
            MonthVM = monthVM;
        }

        public float MonthlyPlusMinus()
        {
            float positiveNumbers=0;
            float negativeNumbers=0;
            foreach(var money in MonthVM.MoneyTrans)
            {
                if (money.IsThisIncome) { positiveNumbers += money.CalculatedValue; }
                else {  negativeNumbers += money.CalculatedValue; }
            }
            return (positiveNumbers + negativeNumbers);
        }
    }
}
