namespace MoneyFox.Infrastructure.DataAccess
{

    using System.Threading;
    using System.Threading.Tasks;
    using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using Core.Common.Interfaces;

    internal sealed class CategoryRepository : ICategoryRepository
    {
        private readonly IAppDbContext appDbContext;

        public CategoryRepository(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await appDbContext.AddAsync(entity: category, cancellationToken: cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        }
    }

}
