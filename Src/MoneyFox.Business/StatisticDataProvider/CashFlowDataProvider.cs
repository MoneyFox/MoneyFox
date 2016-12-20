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
        public CashFlow GetCashFlow(DateTime startDate, DateTime endDate)
        {
            return GetCashFlowStatisticItems(paymentRepository
                .GetList(x => x.Type != PaymentType.Transfer
                              && x.Date.Date >= startDate.Date
                              && x.Date.Date <= endDate.Date)
                .ToList());
        }

        public List<CashFlow> GetCashFlowList(DateTime startDate, DateTime endDate)
        {
            List<CashFlow> cashFlows = new List<CashFlow>();
            var tempDate = startDate;

            while (endDate.Date >= tempDate.Date)
            {
                var date = tempDate;
                var cashFlow = GetCashFlowStatisticItems(paymentRepository
                       .GetList(x => (x.Type != PaymentType.Transfer)
                                     && (x.Date.Date >= date.Date)
                                     && (x.Date.Date <= date.GetLastDayOfMonth()))
                       .ToList());

                cashFlow.Label = date.ToString("MM yy")
                                         + ": +" + cashFlow.Income.Value.ToString("C")
                                         + " / -" + cashFlow.Expense.Value.ToString("C")
                                         + " / " + cashFlow.Revenue.Value.ToString("C");

                cashFlow.Month = date.Month.ToString("d2") + " " + date.Year.ToString("D2");
                cashFlows.Add(cashFlow);
                tempDate = tempDate.AddMonths(1);
            }

            return cashFlows;
        }

        private CashFlow GetCashFlowStatisticItems(List<PaymentViewModel> payments)
        {
            var income = new StatisticItem
            {
                Category = Strings.RevenueLabel,
                Value = payments.Where(x => x.Type == PaymentType.Income).Sum(x => x.Amount)
            };
            income.Label = income.Category + ": " +
                           Math.Round(income.Value, 2, MidpointRounding.AwayFromZero).ToString("C");

            var spent = new StatisticItem
            {
                Category = Strings.ExpenseLabel,
                Value = payments.Where(x => x.Type == PaymentType.Expense).Sum(x => x.Amount)
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
                Expense = spent,
                Revenue = increased
            };
        }
    }
}