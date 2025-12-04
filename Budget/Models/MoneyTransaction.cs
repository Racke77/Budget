using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Calculations;

namespace Budget.Models
{
    public class MoneyTransaction
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public float Money { get; set; }
        public bool IsThisIncome { get; set; } = false;
        public bool Reocurring { get; set; } = false;
        public bool IsThisSalary { get; set; } = false;
        public int SickDays { get; set; }
        public float CalculatedValue { get; set; } //otherwise it will just keep updating forever
        public MoneyTransaction()
        {
            CalculateValue();
        }

        public void CalculateValue()
        {
            if (IsThisIncome == false && CalculatedValue > 0)
            {
                CalculatedValue *= -1; //modify to negative if expense
            }
            else if(IsThisIncome && CalculatedValue < 0)
            {
                CalculatedValue *= -1; //modify to positive if accidental negative
            }
            if (IsThisSalary && Money == CalculatedValue)
            {
                var salaryCalculator = new CalculateSalary(Money); //send in base-money
                CalculatedValue = salaryCalculator.SalaryCalc(SickDays); //update with calculated value
            }
        }
    }
}
