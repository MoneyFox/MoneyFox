namespace MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
}
