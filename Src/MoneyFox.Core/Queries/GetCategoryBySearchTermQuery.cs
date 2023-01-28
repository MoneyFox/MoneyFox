namespace MoneyFox.Core.Queries;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Domain.Aggregates.CategoryAggregate;
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
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<Category>> Handle(GetCategoryBySearchTermQuery request, CancellationToken cancellationToken)
        {
            var categoriesQuery = appDbContext.Categories.OrderBy(x => x.Name);
            var categories = await categoriesQuery.ToListAsync(cancellationToken);
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                categories = categories.WhereNameContains(searchterm: request.SearchTerm).ToList();
            }

            return categories;
        }
    }
}
