namespace MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate
{

    using System.Threading.Tasks;

    public interface IBudgetRepository
    {
        Task AddAsync(Budget budget);

        Task<Budget> GetAsync(int budgetId);

    }

}
