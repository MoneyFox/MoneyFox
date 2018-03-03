using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microcharts;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using SkiaSharp;

namespace MoneyFox.Business.StatisticDataProvider
{
    public class CashFlowDataProvider
    {
        private const string GREEN_HEX_CODE = "#9bcd9b";
        private const string RED_HEX_CODE = "#cd3700";
        private const string BLUE_HEX_CODE = "#87cefa";

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
        public async Task<List<Entry>> GetCashFlow(DateTime startDate, DateTime endDate)
        {
            var paymentEnumerable = await paymentService.GetPaymentsWithoutTransfer(startDate, endDate);
            return GetCashFlowStatisticItems(paymentEnumerable.ToList());
        }

        private List<Entry> GetCashFlowStatisticItems(List<Payment> payments)
        {
            var incomeAmount = (float) payments.Where(x => x.Data.Type == PaymentType.Income).Sum(x => x.Data.Amount);
            var income = new Entry(incomeAmount)
            {
                Label = Strings.RevenueLabel,
                ValueLabel = Math.Round(incomeAmount, 2, MidpointRounding.AwayFromZero).ToString("C"),
                Color = SKColor.Parse(GREEN_HEX_CODE)
            };

            var expenseAmount = (float) payments.Where(x => x.Data.Type == PaymentType.Expense).Sum(x => x.Data.Amount);
            var spent = new Entry(expenseAmount)
            {
                Label = Strings.ExpenseLabel ,
                ValueLabel = Math.Round(expenseAmount, 2, MidpointRounding.AwayFromZero).ToString("C"),
                Color = SKColor.Parse(RED_HEX_CODE)
            };

            var valueIncreased = incomeAmount - expenseAmount;
            var increased = new Entry(valueIncreased)
            {
                Label = Strings.IncreaseLabel,
                ValueLabel = Math.Round(valueIncreased, 2, MidpointRounding.AwayFromZero).ToString("C"),
                Color = SKColor.Parse(BLUE_HEX_CODE)
            };

            return new List<Entry> {income, spent, increased};
        }
    }
}