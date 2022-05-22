namespace MoneyFox.Core.ApplicationCore.Queries
{

    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates.CategoryAggregate;
    using MediatR;
    using MoneyFox.Core.Common.Interfaces;

    public class GetCategoryByIdQuery : IRequest<Category?>
    {
        public GetCategoryByIdQuery(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; }

        public class Handler : IRequestHandler<GetCategoryByIdQuery, Category?>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                return await contextAdapter.Context.Categories.FindAsync(request.CategoryId);
            }
        }
    }

}
