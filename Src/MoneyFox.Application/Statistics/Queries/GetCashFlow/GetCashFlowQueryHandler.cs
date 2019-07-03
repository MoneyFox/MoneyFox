using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Application.Statistics.Models;
using MoneyFox.BusinessDbAccess.QueryObjects;
using MoneyFox.Domain;

namespace MoneyFox.Application.Statistics.Queries.GetCashFlow
{
    public class GetCashFlowQueryHandler : IRequestHandler<GetCashFlowQuery, List<StatisticEntry>>
    {
        private const string GREEN_HEX_CODE = "#9bcd9b";
        private const string RED_HEX_CODE = "#cd3700";
        private const string BLUE_HEX_CODE = "#87cefa";

        private readonly IEfCoreContext context;

        public GetCashFlowQueryHandler(IEfCoreContext context)
        {
            this.context = context;
        }

        public async Task<List<StatisticEntry>> Handle(GetCashFlowQuery request, CancellationToken cancellationToken)
        {
            var payments = await context.Payments
                .Include(x => x.Category)
                .WithoutTransfers()
                .HasDateLargerEqualsThan(request.StartDate.Date)
                .HasDateSmallerEqualsThan(request.EndDate.Date)
                .ToListAsync(cancellationToken);

            var incomeAmount = (float)payments.Where(x => x.Type == PaymentType.Income).Sum(x => x.Amount);
            var income = new StatisticEntry(incomeAmount)
            {
                Label = Strings.RevenueLabel,
                ValueLabel = Math.Round(incomeAmount, 2, MidpointRounding.AwayFromZero).ToString("C", CultureInfo.InvariantCulture),
                Color = GREEN_HEX_CODE
            };

            var expenseAmount = (float)payments.Where(x => x.Type == PaymentType.Expense).Sum(x => x.Amount);
            var spent = new StatisticEntry(expenseAmount)
            {
                Label = Strings.ExpenseLabel,
                ValueLabel = Math.Round(expenseAmount, 2, MidpointRounding.AwayFromZero).ToString("C", CultureInfo.InvariantCulture),
                Color = RED_HEX_CODE
            };

            var valueIncreased = incomeAmount - expenseAmount;
            var increased = new StatisticEntry(valueIncreased)
            {
                Label = Strings.IncreaseLabel,
                ValueLabel = Math.Round(valueIncreased, 2, MidpointRounding.AwayFromZero).ToString("C", CultureInfo.InvariantCulture),
                Color = BLUE_HEX_CODE
            };

            return new List<StatisticEntry> { income, spent, increased };
        }
    }
}
