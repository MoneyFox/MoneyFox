namespace MoneyFox.Core.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetPaymentsForCategorySummary
{
    public class Query : IRequest<List<Payment>>
    {
        public Query(int? categoryId, DateTime dateRangeFrom, DateTime dateRangeTo)
        {
            CategoryId = categoryId;
            DateRangeFrom = dateRangeFrom;
            DateRangeTo = dateRangeTo;
        }

        public int? CategoryId { get; set; }
        public DateTime DateRangeFrom { get; set; }
        public DateTime DateRangeTo { get; set; }
    }

    public class Handler : IRequestHandler<Query, List<Payment>>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<Payment>> Handle(Query request, CancellationToken cancellationToken)
        {
            IQueryable<Payment> query = appDbContext.Payments.Include(x => x.Category);
            query = request.CategoryId.HasValue
                ? query.Where(x => x.Category!.Id == request.CategoryId)
                : query.Where(x => x.Category == null);

            return await query.Where(x => x.Date >= request.DateRangeFrom)
                .Where(x => x.Date <= request.DateRangeTo)
                .WithoutTransfers()
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
