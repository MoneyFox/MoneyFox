using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Payments.Queries.GetMonthlyIncome
{
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

            public async Task<decimal> Handle(GetMonthlyExpenseQuery request, CancellationToken cancellationToken)
            {
                return (await contextAdapter.Context
                                        .Payments
                                        .AreAfterOrEqual(HelperFunctions.GetFirstDayMonth(systemDateHelper))
                                        .AreBeforeOrEqual(HelperFunctions.GetEndOfMonth(systemDateHelper))
                                        .IsExpense()
                                        .Select(x => x.Amount)
                                        .ToListAsync(cancellationToken))
                                        .Sum();
            }
        }
    }
}
