namespace MoneyFox.Core.Queries.Categories.GetIfCategoryWithNameExists
{
    using _Pending_.Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

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

            public async Task<bool> Handle(GetIfCategoryWithNameExistsQuery request,
                CancellationToken cancellationToken)
                => await context.Categories.AnyAsync(x => x.Name == request.CategoryName, cancellationToken);
        }
    }
}