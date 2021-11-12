using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Payments.Queries.GetUnclearedPaymentsOfThisMonth
{
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

            public async Task<List<Payment>> Handle(GetUnclearedPaymentsOfThisMonthQuery request,
                CancellationToken cancellationToken)
            {
                var query = contextAdapter.Context
                                          .Payments
                                          .Include(x => x.ChargedAccount)
                                          .Include(x => x.TargetAccount)
                                          .AreNotCleared()
                                          .HasDateSmallerEqualsThan(
                                              HelperFunctions.GetEndOfMonth(systemDateHelper));

                if(request.AccountId != 0)
                {
                    query = query.HasAccountId(request.AccountId);
                }

                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}