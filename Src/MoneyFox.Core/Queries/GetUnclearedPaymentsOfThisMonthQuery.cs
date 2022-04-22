namespace MoneyFox.Core.Queries
{

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using _Pending_;
    using _Pending_.Common;
    using _Pending_.Common.QueryObjects;
    using Aggregates.AccountAggregate;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetUnclearedPaymentsOfThisMonthQuery : IRequest<List<Payment>>
    {
        public int AccountId { get; set; }

        public class Handler : IRequestHandler<GetUnclearedPaymentsOfThisMonthQuery, List<Payment>>
        {
            private readonly IContextAdapter contextAdapter;
            private readonly ISystemDateHelper systemDateHelper;

            public Handler(IContextAdapter contextAdapter, ISystemDateHelper systemDateHelper)
            {
                this.contextAdapter = contextAdapter;
                this.systemDateHelper = systemDateHelper;
            }

            public async Task<List<Payment>> Handle(GetUnclearedPaymentsOfThisMonthQuery request, CancellationToken cancellationToken)
            {
                var query = contextAdapter.Context.Payments.Include(x => x.ChargedAccount)
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

}
