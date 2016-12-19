using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MoneyFox.Business.Extensions;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Business.StatisticDataProvider
{
    public class MonthlyExpensesDataProvider : IStatisticProvider<IEnumerable<CashFlow>>
    {
        private readonly IPaymentRepository paymentRepository;

        public MonthlyExpensesDataProvider(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public IEnumerable<CashFlow> GetValues(DateTime startDate, DateTime endDate)
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

                cashFlow.CashFlowLabel = date.ToString("yyyy MM")
                                         + "\n Income: " + cashFlow.Income.Value
                                         + "\n Expense: " + cashFlow.Expense.Value
                                         + "\n Revenue: " + cashFlow.Revenue.Value;

                cashFlows.Add(cashFlow);

                tempDate = tempDate.AddMonths(1);
            }

            return cashFlows;
        }

        //TODO Refactor this to use the same logic as in cashFlow Provider
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