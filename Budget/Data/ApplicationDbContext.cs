using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.Data
{
    public class ApplicationDbContext : DbContext
    {
        private string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BudgetWPF;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public DbSet<MoneyTransaction> MoneyTransactions { get; set; }
        public DbSet<Month> Months { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var test = optionsBuilder.UseSqlite(ConnectionString);
        }
    }
}
