using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Common.QueryObjects;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Queries.Statistics.Queries
{
    public class GetCashFlowQuery : IRequest<List<StatisticEntry>>
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    public class GetCashFlowQueryHandler : IRequestHandler<GetCashFlowQuery, List<StatisticEntry>>
    {
        private const string GREEN_HEX_CODE = "#9bcd9b";
        private const string RED_HEX_CODE = "#cd3700";
        private const string BLUE_HEX_CODE = "#87cefa";

        private readonly IContextAdapter contextAdapter;

        public GetCashFlowQueryHandler(IContextAdapter contextAdapter)
        {
            this.contextAdapter = contextAdapter;
        }

        public async Task<List<StatisticEntry>> Handle(GetCashFlowQuery request, CancellationToken cancellationToken)
        {
            List<Payment> payments = await contextAdapter.Context
                .Payments
                .Include(x => x.Category)
                .WithoutTransfers()
                .HasDateLargerEqualsThan(request.StartDate.Date)
                .HasDateSmallerEqualsThan(request.EndDate.Date)
                .ToListAsync(cancellationToken);

            decimal incomeAmount = payments.Where(x => x.Type == PaymentType.Income)
                .Sum(x => x.Amount);
            var income = new StatisticEntry(incomeAmount)
            {
                Label = Strings.RevenueLabel,
                ValueLabel = Math.Round(incomeAmount, 2, MidpointRounding.AwayFromZero)
                    .ToString("C", CultureHelper.CurrentCulture),
                Color = GREEN_HEX_CODE
            };

            decimal expenseAmount = payments.Where(x => x.Type == PaymentType.Expense)
                .Sum(x => x.Amount);
            var spent = new StatisticEntry(expenseAmount)
            {
                Label = Strings.ExpenseLabel,
                ValueLabel = Math.Round(expenseAmount, 2, MidpointRounding.AwayFromZero)
                    .ToString("C", CultureHelper.CurrentCulture),
                Color = RED_HEX_CODE
            };

            decimal valueIncreased = incomeAmount - expenseAmount;
            var increased = new StatisticEntry(valueIncreased)
            {
                Label = Strings.IncreaseLabel,
                ValueLabel = Math.Round(valueIncreased, 2, MidpointRounding.AwayFromZero)
                    .ToString("C", CultureHelper.CurrentCulture),
                Color = BLUE_HEX_CODE
            };

            return new List<StatisticEntry> { income, spent, increased };
        }
    }
}