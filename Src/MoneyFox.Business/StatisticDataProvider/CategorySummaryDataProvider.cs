using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Models;

namespace MoneyFox.Business.StatisticDataProvider
{
    public class CategorySummaryDataProvider : IStatisticProvider<IEnumerable<StatisticItem>>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IPaymentRepository paymentRepository;

        public CategorySummaryDataProvider(IPaymentRepository paymentRepository, ICategoryRepository categoryRepository)
        {
            this.paymentRepository = paymentRepository;
            this.categoryRepository = categoryRepository;
        }

        public IEnumerable<StatisticItem> GetValues(DateTime startDate, DateTime endDate)
        {
            var categories = new ObservableCollection<StatisticItem>();

            foreach (var category in categoryRepository.GetList())
            {
                categories.Add(new StatisticItem
                {
                    Category = category.Name,
                    Value = paymentRepository
                        .GetList()
                        .Where(x => (x.Date.Date >= startDate.Date) && (x.Date.Date <= endDate.Date))
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