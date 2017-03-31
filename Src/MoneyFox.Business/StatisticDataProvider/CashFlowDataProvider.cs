using System;
using System.Collections.Generic;
using System.Linq;
using MoneyFox.Business.Extensions;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Business.StatisticDataProvider
{
    public class CashFlowDataProvider
    {
        private readonly IRepository<PaymentViewModel> paymentRepository;

        public CashFlowDataProvider(IRepository<PaymentViewModel> paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        /// <summary>
        ///     Selects payments from the given timeframe and calculates the income, the spendings and the revenue.
        ///     There is no differentiation between the accounts, therefore transfers are ignored.
        /// </summary>
        /// <param name="startDate">Startpoint form which to select data.</param>
        /// <param name="endDate">Endpoint form which to select data.</param>
        /// <returns>Statistic value for the given timeframe</returns>
        public List<StatisticItem> GetCashFlow(DateTime startDate, DateTime endDate)
        {
            return GetCashFlowStatisticItems(paymentRepository
                .GetList(x => x.Type != PaymentType.Transfer
                              && x.Date.Date >= startDate.Date
                              && x.Date.Date <= endDate.Date)
                .ToList());
        }

        private List<StatisticItem> GetCashFlowStatisticItems(List<PaymentViewModel> payments)
        {
            var income = new StatisticItem
            {
                Value = payments.Where(x => x.Type == PaymentType.Income).Sum(x => x.Amount)
            };
            income.Label = Strings.RevenueLabel + ": " +
                           Math.Round(income.Value, 2, MidpointRounding.AwayFromZero).ToString("C");

            var spent = new StatisticItem
            {
                Value = payments.Where(x => x.Type == PaymentType.Expense).Sum(x => x.Amount)
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