namespace MoneyFox.Core.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetIfCategoryWithNameExistsQuery : IRequest<bool>
{
    public GetIfCategoryWithNameExistsQuery(string categoryName)
    {
        CategoryName = categoryName;
    }

    public string CategoryName { get; }

    public class Handler : IRequestHandler<GetIfCategoryWithNameExistsQuery, bool>
    {
        private readonly IAppDbContext context;

        public Handler(IAppDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> Handle(GetIfCategoryWithNameExistsQuery request, CancellationToken cancellationToken)
        {
            return await context.Categories.AnyAsync(predicate: x => x.Name == request.CategoryName, cancellationToken: cancellationToken);
        }
    }
}
