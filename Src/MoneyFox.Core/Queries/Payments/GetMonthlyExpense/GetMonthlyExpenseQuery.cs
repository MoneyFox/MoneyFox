namespace MoneyFox.Core.Queries.Payments.GetMonthlyExpense
{
    using _Pending_;
    using _Pending_.Common;
    using _Pending_.Common.Interfaces;
    using _Pending_.Common.QueryObjects;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetMonthlyExpenseQuery : IRequest<decimal>
    {
        public class Handler : IRequestHandler<GetMonthlyExpenseQuery, decimal>
        {
            private readonly IContextAdapter contextAdapter;
            private readonly ISystemDateHelper systemDateHelper;

            public Handler(IContextAdapter contextAdapter, ISystemDateHelper systemDateHelper)
            {
                this.contextAdapter = contextAdapter;
                this.systemDateHelper = systemDateHelper;
            }

            public async Task<decimal> Handle(GetMonthlyExpenseQuery request, CancellationToken cancellationToken) =>
                (await contextAdapter.Context
                    .Payments
                    .HasDateLargerEqualsThan(HelperFunctions.GetFirstDayMonth(systemDateHelper))
                    .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth(systemDateHelper))
                    .IsExpense()
                    .Select(x => x.Amount)
                    .ToListAsync(cancellationToken))
                .Sum();
        }
    }
}