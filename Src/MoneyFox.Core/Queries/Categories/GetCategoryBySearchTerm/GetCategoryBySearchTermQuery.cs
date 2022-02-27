namespace MoneyFox.Core.Queries.Categories.GetCategoryBySearchTerm
{
    using _Pending_.Common.Interfaces;
    using _Pending_.Common.QueryObjects;
    using Aggregates.Payments;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetCategoryBySearchTermQuery : IRequest<List<Category>>
    {
        public GetCategoryBySearchTermQuery(string searchTerm = "")
        {
            SearchTerm = searchTerm;
        }

        public string SearchTerm { get; }

        public class Handler : IRequestHandler<GetCategoryBySearchTermQuery, List<Category>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<List<Category>> Handle(GetCategoryBySearchTermQuery request,
                CancellationToken cancellationToken)
            {
                IOrderedQueryable<Category> categoriesQuery = contextAdapter.Context
                    .Categories
                    .OrderBy(x => x.Name);

                List<Category>? categories = await categoriesQuery.ToListAsync(cancellationToken);

                if(!string.IsNullOrEmpty(request.SearchTerm))
                {
                    categories = categories.WhereNameContains(request.SearchTerm)
                        .ToList();
                }

                return categories;
            }
        }
    }
}