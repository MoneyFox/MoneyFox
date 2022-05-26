namespace MoneyFox.Infrastructure.DataAccess
{

    using System.Threading;
    using System.Threading.Tasks;
    using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using Persistence;

    internal sealed class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext appDbContext;
        public CategoryRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await appDbContext.AddAsync(category, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken:cancellationToken);
        }
    }
}
