namespace MoneyFox.Core.ApplicationCore.Queries;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _Pending_.Common;
using _Pending_.Common.QueryObjects;
using Common.Helpers;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetUnclearedPaymentsOfThisMonthQuery : IRequest<List<Payment>>
{
    public int AccountId { get; set; }

    public class Handler : IRequestHandler<GetUnclearedPaymentsOfThisMonthQuery, List<Payment>>
    {
        private readonly IAppDbContext appDbContext;
        private readonly ISystemDateHelper systemDateHelper;

        public Handler(IAppDbContext appDbContext, ISystemDateHelper systemDateHelper)
        {
            this.appDbContext = appDbContext;
            this.systemDateHelper = systemDateHelper;
        }

        public async Task<List<Payment>> Handle(GetUnclearedPaymentsOfThisMonthQuery request, CancellationToken cancellationToken)
        {
            var query = appDbContext.Payments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .AreNotCleared()
                .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth(systemDateHelper));

            if (request.AccountId != 0)
            {
                query = query.HasAccountId(request.AccountId);
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
