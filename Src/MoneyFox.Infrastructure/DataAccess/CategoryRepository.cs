namespace MoneyFox.Infrastructure.DataAccess
{

    using System.Threading.Tasks;
    using Core.Aggregates.CategoryAggregate;

    internal sealed class CategoryRepository : ICategoryRepository
    {
        public Task AddAsync(Category category)
        {
            throw new System.NotImplementedException();
        }
    }
}
