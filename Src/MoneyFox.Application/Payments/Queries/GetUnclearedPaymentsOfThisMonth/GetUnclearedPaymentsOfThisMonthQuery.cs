using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using MoneyFox.Application.Common.QueryObjects;

namespace MoneyFox.Application.Payments.Queries.GetUnclearedPaymentsOfThisMonth
{
    public class GetUnclearedPaymentsOfThisMonthQuery : IRequest<List<Payment>>
    {
        public int AccountId { get; set; }

        public class Handler : IRequestHandler<GetUnclearedPaymentsOfThisMonthQuery, List<Payment>>
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<List<Payment>> Handle(GetUnclearedPaymentsOfThisMonthQuery request, CancellationToken cancellationToken)
            {
                IQueryable<Payment> query = context.Payments
                                                   .Include(x => x.ChargedAccount)
                                                   .Include(x => x.TargetAccount)
                                                   .AreNotCleared()
                                                   .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth());

                if (request.AccountId != 0) query = query.HasAccountId(request.AccountId);

                return await query.ToListAsync(cancellationToken);
            }
        }
    }
}
