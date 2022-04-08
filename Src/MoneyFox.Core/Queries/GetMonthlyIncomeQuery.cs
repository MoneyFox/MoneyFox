namespace MoneyFox.Core.Queries
{

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using _Pending_;
    using _Pending_.Common;
    using _Pending_.Common.QueryObjects;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

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

            public async Task<decimal> Handle(GetMonthlyIncomeQuery request, CancellationToken cancellationToken)
            {
                return (await contextAdapter.Context.Payments.HasDateLargerEqualsThan(HelperFunctions.GetFirstDayMonth(systemDateHelper))
                    .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth(systemDateHelper))
                    .IsIncome()
                    .Select(x => x.Amount)
                    .ToListAsync(cancellationToken)).Sum();
            }
        }
    }

}
