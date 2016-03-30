using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Model;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation;

namespace MoneyManager.Core.StatisticDataProvider
{
    public class MonthlyExpensesDataProvider : IStatisticProvider<IEnumerable<StatisticItem>>
    {
        private readonly IPaymentRepository paymentRepository;

        public MonthlyExpensesDataProvider(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public IEnumerable<StatisticItem> GetValues(DateTime startDate, DateTime endDate)
        {
            return paymentRepository.Data
                .Where(x => x.Type == (int) PaymentType.Expense)
                .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date)
                .GroupBy(x => x.Date.ToString("MMMM", CultureInfo.InvariantCulture))
                .Select(group => new StatisticItem
                {
                    Category = group.Key,
                    Label = group.Key + ": " + group.ToList().Sum(x => x.Amount).ToString("C"),
                    Value = group.ToList().Sum(x => x.Amount)
                })
                .ToList();
        }
    }
}