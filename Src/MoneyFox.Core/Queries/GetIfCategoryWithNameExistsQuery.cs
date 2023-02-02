namespace MoneyFox.Core.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Domain.Aggregates.CategoryAggregate;

public class GetIfCategoryWithNameExistsQuery : IRequest<bool>
{
    public GetIfCategoryWithNameExistsQuery(string categoryName, int categoryId)
    {
        CategoryName = categoryName;
        CategoryId = categoryId;
    }

    public string CategoryName { get; }
    public int CategoryId { get; }

    public class Handler : IRequestHandler<GetIfCategoryWithNameExistsQuery, bool>
    {
        private readonly IAppDbContext context;

        public Handler(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(GetIfCategoryWithNameExistsQuery request, CancellationToken cancellationToken)
        {
            return await context.Categories.AnyAsync(predicate: x => x.Name == request.CategoryName && x.Id != request.CategoryId, cancellationToken: cancellationToken);

        }
    }
}
