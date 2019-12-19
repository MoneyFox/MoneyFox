using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Application.Categories.Queries.GetIfCategoryWithNameExists
{
    public class GetIfCategoryWithNameExistsQuery : IRequest<bool>
    {
        public GetIfCategoryWithNameExistsQuery(string categoryName)
        {
            CategoryName = categoryName;
        }

        public string CategoryName { get; }

        public class Handler : IRequestHandler<GetIfCategoryWithNameExistsQuery, bool>
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<bool> Handle(GetIfCategoryWithNameExistsQuery request, CancellationToken cancellationToken)
            {
                return await context.Categories.AnyAsync(x => x.Name == request.CategoryName, cancellationToken);
            }
        }
    }
}
