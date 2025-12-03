using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.Data
{
    public class BudgetRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public BudgetRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        //CREATE
        public async Task CreateMonth(Month month)
        {
            await applicationDbContext.Months.AddAsync(month);
            await applicationDbContext.SaveChangesAsync();
        }
        public async Task CreateMoneyTransaction(MoneyTransaction money)
        {
            await applicationDbContext.MoneyTransactions.AddAsync(money);
            await applicationDbContext.SaveChangesAsync();
        }

        //READ
        public async Task<List<Month>> GetAllMonthsAsync()
        {
            return await applicationDbContext.Months
                .Include(m => m.ListMoney)
                .ToListAsync();
        }
        public async Task<List<MoneyTransaction>> GetAllTransactionsByMonth()
        {
            return await applicationDbContext.MoneyTransactions.ToListAsync();
        }

        //UPDATE
        public async Task UpdateMonth(Month month)
        {
            applicationDbContext.Months.Update(month);
            await applicationDbContext.SaveChangesAsync();
        }
        public async Task UpdateMoneyTransaction(MoneyTransaction money)
        {
            applicationDbContext.MoneyTransactions.Update(money);
            await applicationDbContext.SaveChangesAsync();
        }

        //DELETE
        public async Task DeleteMonth(Month month)
        {
            applicationDbContext.Months.Remove(month);
            applicationDbContext.SaveChanges();
        }
        public async Task DeleteMoneyTransaction(MoneyTransaction money)
        {
            applicationDbContext.MoneyTransactions.Remove(money);
            applicationDbContext.SaveChanges();
        }
    }
}
