using MediatR;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Payments.Queries.GetMonthlyIncome
{
    public class GetMonthlyIncomeQuery : IRequest<decimal>
    {
        public class Handler : IRequestHandler<GetMonthlyIncomeQuery, decimal>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<decimal> Handle(GetMonthlyIncomeQuery request, CancellationToken cancellationToken)
            {
                var sum = contextAdapter.Context
                                        .Payments
                                        .HasDateLargerEqualsThan(HelperFunctions.GetFirstDayMonth())
                                        .HasDateSmallerEqualsThan(HelperFunctions.GetEndOfMonth())
                                        .IsExpense()
                                        .Sum(x => x.Amount);

                return await Task.FromResult(sum);
            }
        }
    }
}
