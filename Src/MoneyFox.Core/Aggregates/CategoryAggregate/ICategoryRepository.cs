namespace MoneyFox.Core.Aggregates.CategoryAggregate
{

    using System.Threading.Tasks;

    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
    }

}
