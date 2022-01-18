using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_;
using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Common.QueryObjects;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Queries.Payments.GetMonthlyIncome
{
    public class GetMonthlyIncomeQuery : IRequest<decimal>
    {
        public class Handler : IRequestHandler<GetMonthlyIncomeQuery, decimal>
        {
            private readonly IContextAdapter contextAdapter;
            private readonly ISystemDateHelper systemDateHelper;

            public Handler(IContextAdapter contextAdapter, ISystemDateHelper systemDateHelper)
            {
                this.contextAdapter = contextAdapter;
                this.systemDateHelper = systemDateHelper;
            }

            public async Task<decimal> Handle(GetMonthlyIncomeQuery request, CancellationToken cancellationToken) =>
                (await contextAdapter.Context
                                     .Payments
                                     .HasDateLargerEqualsThan(HelperFunctions.GetFirstDayMonth(systemDateHelper))
                                     .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth(systemDateHelper))
                                     .IsIncome()
                                     .Select(x => x.Amount)
                                     .ToListAsync(cancellationToken))
                .Sum();
        }
    }
}