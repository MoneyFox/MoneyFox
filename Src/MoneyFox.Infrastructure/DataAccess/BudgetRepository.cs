namespace MoneyFox.Infrastructure.DataAccess
{

    using System.Threading.Tasks;
    using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using Persistence;

    internal sealed class BudgetRepository : IBudgetRepository
    {
        private readonly AppDbContext appDbContext;
        public BudgetRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Task AddAsync(Budget budget)
        {
            throw new System.NotImplementedException();
        }

        public Task<Budget> GetAsync(int budgetId)
        {
            throw new System.NotImplementedException();
        }
    }

}
