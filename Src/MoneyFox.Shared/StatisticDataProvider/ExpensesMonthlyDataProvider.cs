using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.StatisticDataProvider
{
    public class MonthlyExpensesDataProvider : IStatisticProvider<IEnumerable<StatisticItem>>, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;

        public MonthlyExpensesDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public IEnumerable<StatisticItem> GetValues(DateTime startDate, DateTime endDate)
            => unitOfWork.PaymentRepository.Data
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