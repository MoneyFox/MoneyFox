using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates.Payments;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Queries.Categories.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<Category>
    {
        public GetCategoryByIdQuery(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; }

        public class Handler : IRequestHandler<GetCategoryByIdQuery, Category>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Category> Handle(GetCategoryByIdQuery request,
                CancellationToken cancellationToken) =>
                await contextAdapter.Context.Categories.FindAsync(request.CategoryId);
        }
    }
}