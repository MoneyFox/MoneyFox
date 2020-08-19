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

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<decimal> Handle(GetMonthlyExpenseQuery request, CancellationToken cancellationToken)
            {
                return (await contextAdapter.Context
                                        .Payments
                                        .HasDateLargerEqualsThan(HelperFunctions.GetFirstDayMonth())
                                        .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth())
                                        .IsExpense()
                                        .Select(x => x.Amount)
                                        .ToListAsync(cancellationToken))
                                        .Sum();
            }
        }
    }
}
