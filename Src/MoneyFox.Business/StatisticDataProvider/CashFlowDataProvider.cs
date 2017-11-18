using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service.DataServices;

namespace MoneyFox.Business.StatisticDataProvider
{
    public class CashFlowDataProvider
    {
        private readonly IPaymentService paymentService;

        public CashFlowDataProvider(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        /// <summary>
        ///     Selects payments from the given timeframe and calculates the income, the spendings and the revenue.
        ///     There is no differentiation between the accounts, therefore transfers are ignored.
        /// </summary>
        /// <param name="startDate">Startpoint form which to select data.</param>
        /// <param name="endDate">Endpoint form which to select data.</param>
        /// <returns>Statistic value for the given timeframe</returns>
        public async Task<List<StatisticItem>> GetCashFlow(DateTime startDate, DateTime endDate)
        {
            var paymentEnumerable = await paymentService.GetPaymentsWithoutTransfer(startDate, endDate);
            return GetCashFlowStatisticItems(paymentEnumerable.ToList());
        }

        private List<StatisticItem> GetCashFlowStatisticItems(List<Payment> payments)
        {
            var income = new StatisticItem
            {
                Value = payments.Where(x => x.Data.Type == PaymentType.Income).Sum(x => x.Data.Amount)
            };
            income.Label = Strings.RevenueLabel + ": " +
                           Math.Round(income.Value, 2, MidpointRounding.AwayFromZero).ToString("C");

            var spent = new StatisticItem
            {
                Value = payments.Where(x => x.Data.Type == PaymentType.Expense).Sum(x => x.Data.Amount)
            };
            spent.Label = Strings.ExpenseLabel + ": " +
                          Math.Round(spent.Value, 2, MidpointRounding.AwayFromZero).ToString("C");

            var increased = new StatisticItem
            {
                Value = income.Value - spent.Value
            };
            increased.Label = Strings.IncreaseLabel + ": " +
                              Math.Round(increased.Value, 2, MidpointRounding.AwayFromZero).ToString("C");

            return new List<StatisticItem> {income, spent, increased};
        }
    }
}