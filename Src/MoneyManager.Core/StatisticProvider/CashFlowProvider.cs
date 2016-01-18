using System;
using System.Collections.Generic;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;

namespace MoneyManager.Core.StatisticProvider
{
    public class CashFlowProvider : IStatisticProvider<CashFlow>
    {
        private readonly IPaymentRepository paymentRepository;

        public CashFlowProvider(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        /// <summary>
        ///     Selects transactions from the given timeframe and calculates the income, the spendings and the revenue.
        ///     There is no differentiation between the accounts, therefore transfers are ignored.
        /// </summary>
        /// <param name="startDate">Startpoint form which to select data.</param>
        /// <param name="endDate">Endpoint form which to select data.</param>
        /// <returns>Statistic value for the given timeframe</returns>
        public CashFlow GetValues(DateTime startDate, DateTime endDate)
        {
            var transactionListFunc =
                new Func<List<Payment>>(() =>
                    paymentRepository.Data
                        .Where(x => x.Type != (int) PaymentType.Transfer)
                        .Where(x => x.Date >= startDate.Date && x.Date <= endDate.Date)
                        .ToList());

            return GetCashFlowStatisticItems(transactionListFunc);
        }

        private CashFlow GetCashFlowStatisticItems(
            Func<List<Payment>> getTransactionListFunc)
        {
            var transactionList = getTransactionListFunc();

            var income = new StatisticItem
            {
                Category = Strings.RevenueLabel,
                Value = transactionList.Where(x => x.Type == (int) PaymentType.Income).Sum(x => x.Amount)
            };
            income.Label = income.Category + ": " +
                           Math.Round(income.Value, 2, MidpointRounding.AwayFromZero).ToString("C");

            var spent = new StatisticItem
            {
                Category = Strings.ExpenseLabel,
                Value = transactionList.Where(x => x.Type == (int) PaymentType.Spending).Sum(x => x.Amount)
            };
            spent.Label = spent.Category + ": " +
                          Math.Round(spent.Value, 2, MidpointRounding.AwayFromZero).ToString("C");

            var increased = new StatisticItem
            {
                Category = Strings.IncreaseLabel,
                Value = income.Value - spent.Value
            };
            increased.Label = increased.Category + ": " +
                              Math.Round(increased.Value, 2, MidpointRounding.AwayFromZero).ToString("C");

            return new CashFlow
            {
                Income = income,
                Spending = spent,
                Revenue = increased
            };
        }
    }
}