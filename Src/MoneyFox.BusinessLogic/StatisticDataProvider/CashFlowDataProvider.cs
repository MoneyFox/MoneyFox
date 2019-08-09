using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.BusinessDbAccess.StatisticDataProvider;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.BusinessLogic.StatisticDataProvider
{
    public interface ICashFlowDataProvider
    {
        Task<List<StatisticEntry>> GetCashFlow(DateTime startDate, DateTime endDate);
    }

    public class CashFlowDataProvider : ICashFlowDataProvider
    {
        private const string GREEN_HEX_CODE = "#9bcd9b";
        private const string RED_HEX_CODE = "#cd3700";
        private const string BLUE_HEX_CODE = "#87cefa";

        private readonly IStatisticDbAccess statisticDbAccess;

        public CashFlowDataProvider(IStatisticDbAccess statisticDbAccess)
        {
            this.statisticDbAccess = statisticDbAccess;
        }

        /// <summary>
        ///     Selects payments from the given time frame and calculates the income, the spendings and the revenue.
        ///     There is no differentiation between the accounts, therefore transfers are ignored.
        /// </summary>
        /// <param name="startDate">Start point form which to select data.</param>
        /// <param name="endDate">Endpoint form which to select data.</param>
        /// <returns>Statistic value for the given time frame</returns>
        public async Task<List<StatisticEntry>> GetCashFlow(DateTime startDate, DateTime endDate)
        {
            return GetCashFlowStatisticItems(await statisticDbAccess.GetPaymentsWithoutTransfer(startDate, endDate)
                                                                    );
        }

        private List<StatisticEntry> GetCashFlowStatisticItems(IReadOnlyCollection<Payment> payments)
        {
            var incomeAmount = (float) payments.Where(x => x.Type == PaymentType.Income).Sum(x => x.Amount);
            var income = new StatisticEntry(incomeAmount)
            {
                Label = Strings.RevenueLabel,
                ValueLabel = Math.Round(incomeAmount, 2, MidpointRounding.AwayFromZero).ToString("C", CultureInfo.InvariantCulture),
                Color = GREEN_HEX_CODE
            };

            var expenseAmount = (float) payments.Where(x => x.Type == PaymentType.Expense).Sum(x => x.Amount);
            var spent = new StatisticEntry(expenseAmount)
            {
                Label = Strings.ExpenseLabel ,
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

            return new List<StatisticEntry> {income, spent, increased};
        }
    }
}