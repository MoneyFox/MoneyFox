namespace MoneyFox.Infrastructure.DataAccess
{

    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    internal sealed class BudgetRepository : IBudgetRepository
    {
        private readonly AppDbContext appDbContext;

        public BudgetRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddAsync(Budget budget)
        {
            await appDbContext.Budgets.AddAsync(budget);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<Budget> GetAsync(int budgetId)
        {
            return await appDbContext.Budgets.Where(b => b.Id == budgetId).SingleAsync();
        }

        public async Task<IReadOnlyCollection<Budget>> GetAsync()
        {
            return await appDbContext.Budgets.ToListAsync();
        }

        public Task UpdateAsync(Budget budget)
        {
            throw new System.NotImplementedException();
        }
    }

}
