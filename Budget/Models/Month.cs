using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Models
{
    public class Month
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MoneyTransaction> ListMoney { get; set; } = new List<MoneyTransaction>();
        public Month()
        {
            
        }
    }
}
