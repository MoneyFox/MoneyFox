namespace MoneyFox.Infrastructure.DataAccess
{

    using System.Threading;
    using System.Threading.Tasks;
    using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using Persistence;

    internal sealed class CategoryRepository : ICategoryRepository
    {
        private readonly ContextAdapter contextAdapter;
        public CategoryRepository(ContextAdapter contextAdapter)
        {
            this.contextAdapter = contextAdapter;
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            var appDbContext = contextAdapter.Context;

            await appDbContext.AddAsync(category, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken:cancellationToken);
        }
    }
}
