namespace MoneyFox.Core.ApplicationCore.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.CategoryAggregate;
using MediatR;

public class GetCategoryByIdQuery : IRequest<Category?>
{
    public GetCategoryByIdQuery(int categoryId)
    {
        CategoryId = categoryId;
    }

    public int CategoryId { get; }

    public class Handler : IRequestHandler<GetCategoryByIdQuery, Category?>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Categories.FindAsync(request.CategoryId);
        }
    }
}
