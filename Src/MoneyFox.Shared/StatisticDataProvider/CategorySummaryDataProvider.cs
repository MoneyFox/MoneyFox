using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.StatisticDataProvider
{
    public class CategorySummaryDataProvider : IStatisticProvider<IEnumerable<StatisticItem>>
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly IPaymentRepository paymentRepository;

        public CategorySummaryDataProvider(IPaymentRepository paymentRepository,
            IRepository<Category> categoryRepository)
        {
            this.paymentRepository = paymentRepository;
            this.categoryRepository = categoryRepository;
        }

        public IEnumerable<StatisticItem> GetValues(DateTime startDate, DateTime endDate)
        {
            var categories = new ObservableCollection<StatisticItem>();

            foreach (var category in categoryRepository.Data)
            {
                categories.Add(new StatisticItem
                {
                    Category = category.Name,
                    Value = paymentRepository.Data
                        .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date)
                        .Where(x => x.CategoryId == category.Id)
                        .Where(x => x.Type != (int) PaymentType.Transfer)
                        .Sum(x => x.Type == (int) PaymentType.Expense
                            ? -x.Amount
                            : x.Amount)
                });
            }

            return new ObservableCollection<StatisticItem>(
                categories.Where(x => Math.Abs(x.Value) > 0.1).OrderBy(x => x.Value).ToList());
        }
    }
}