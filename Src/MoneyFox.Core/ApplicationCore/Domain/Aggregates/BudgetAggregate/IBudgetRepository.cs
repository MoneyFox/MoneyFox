namespace MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Interfaces;

public interface IBudgetRepository : IRepository<Budget>
{
    Task AddAsync(Budget budget);

    Task<Budget> GetAsync(int budgetId);

    Task<IReadOnlyCollection<Budget>> GetAsync();

    Task UpdateAsync(Budget budget);

    Task DeleteAsync(int testBudgetId);
}
