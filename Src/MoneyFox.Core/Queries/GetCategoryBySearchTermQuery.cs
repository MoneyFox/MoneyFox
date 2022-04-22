namespace MoneyFox.Core.Queries
{

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using _Pending_.Common.QueryObjects;
    using Aggregates;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

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

            public async Task<List<Category>> Handle(GetCategoryBySearchTermQuery request, CancellationToken cancellationToken)
            {
                var categoriesQuery = contextAdapter.Context.Categories.OrderBy(x => x.Name);
                var categories = await categoriesQuery.ToListAsync(cancellationToken);
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    categories = categories.WhereNameContains(request.SearchTerm).ToList();
                }

                return categories;
            }
        }
    }

}
