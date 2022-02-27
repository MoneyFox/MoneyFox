namespace MoneyFox.Core.Queries.Categories.GetCategoryById
{
    using _Pending_.Common.Interfaces;
    using Aggregates.Payments;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

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