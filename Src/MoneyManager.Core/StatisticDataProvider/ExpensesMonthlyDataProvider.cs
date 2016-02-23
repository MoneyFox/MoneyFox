using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

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
                .Where(x => x.Date >= startDate && x.Date <= endDate)
                .GroupBy(x => x.Date.ToString("MMMM", CultureInfo.InvariantCulture))
                .Select(group => new StatisticItem {Category = group.Key, Label = group.Key, Value = group.ToList().Sum(x => x.Amount)})
                .ToList();
        }
    }
}
