using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
    public class MoneyTransaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Money { get; set; }
        public bool IsThisIncome { get; set; } = false;
        public bool Reocurring { get; set; } = false;
        public bool IsThisSalary { get; set; } = false;
        public int SickDays { get; set; }
        public float CalculatedValue { get; set; } //otherwise it will just keep updating forever
        public MoneyTransaction()
        {
            CalculatedValue = Money;
        }
    }
}
