namespace MoneyFox.Core.ApplicationCore.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetPaymentsForCategoryQuery : IRequest<List<Payment>>
{
    public GetPaymentsForCategoryQuery(int categoryId, DateTime dateRangeFrom, DateTime dateRangeTo)
    {
        CategoryId = categoryId;
        DateRangeFrom = dateRangeFrom;
        DateRangeTo = dateRangeTo;
    }

    public int CategoryId { get; set; }
    public DateTime DateRangeFrom { get; set; }
    public DateTime DateRangeTo { get; set; }

    public class Handler : IRequestHandler<GetPaymentsForCategoryQuery, List<Payment>>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<Payment>> Handle(GetPaymentsForCategoryQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Payment> query = appDbContext.Payments.Include(x => x.Category);
            query = request.CategoryId == 0 ? query.Where(x => x.Category == null) : query.Where(x => x.Category!.Id == request.CategoryId);

            return await query.Where(x => x.Date >= request.DateRangeFrom)
                .Where(x => x.Date <= request.DateRangeTo)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
