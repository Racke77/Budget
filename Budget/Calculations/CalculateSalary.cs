using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.ViewModels;

namespace Budget.Calculations
{
    public class CalculateSalary
    {
        public float TotalWorkHours { get; set; } = 2080; //average work hours per year
        public float YearlySalary { get; set; }
        public float PayPercentageWhenSick { get; set; } = 0.8f;
        public float HourlySalary { get; set; }
        public float WorkHoursPerDay { get; set; } = 8;

        public CalculateSalary(float monthlySalary)
        {
            YearlySalary = monthlySalary * 12;
            HourlySalary = YearlySalary / TotalWorkHours;
        }

        public float SalaryCalc(int sickDays)
        {
            //sick days this month
            var sickHours = WorkHoursPerDay * sickDays;
            var sickSalary = HourlySalary * PayPercentageWhenSick * sickHours;
            //non sick-days this month
            var nonSickHours = (TotalWorkHours / 12) - (sickDays * WorkHoursPerDay);
            var nonSickSalary = HourlySalary * nonSickHours;
            //total salary
            var total = MathF.Round(sickSalary + nonSickSalary);
            return total;
        }
        public bool HasSalaryBeenCalculated(MoneyVM moneyVM)
        {
            if (YearlySalary / 12 == moneyVM.Money) { return false; } //same as if it hasn't been calculated
            else { return true; }
        }
    }
}
