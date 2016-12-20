using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Models;

namespace MoneyFox.Business.StatisticDataProvider
{
    public class MonthlyExpensesDataProvider : IStatisticProvider<IEnumerable<StatisticItem>>
    {
        private readonly IPaymentRepository paymentRepository;

        public MonthlyExpensesDataProvider(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public IEnumerable<StatisticItem> GetValues(DateTime startDate, DateTime endDate)
            => paymentRepository
                .GetList(x => x.Type == (int) PaymentType.Expense)
                .Where(x => (x.Date.Date >= startDate.Date) && (x.Date.Date <= endDate.Date))
                .OrderBy(x => x.Date)
                .GroupBy(x => x.Date.ToString("yyyy MMMM", CultureInfo.InvariantCulture))
                .Select(group => new StatisticItem
                {
                    Category = group.Key,
                    Label = group.Key + ": " + group.ToList().Sum(x => x.Amount).ToString("C"),
                    Value = group.ToList().Sum(x => x.Amount)
                })
                .ToList();
    }
}