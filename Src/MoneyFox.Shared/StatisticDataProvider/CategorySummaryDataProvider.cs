using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.StatisticDataProvider
{
    public class CategorySummaryDataProvider : IStatisticProvider<IEnumerable<StatisticItem>>, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;

        public CategorySummaryDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        public IEnumerable<StatisticItem> GetValues(DateTime startDate, DateTime endDate)
        {
            var categories = new ObservableCollection<StatisticItem>();

            foreach (var category in unitOfWork.CategoryRepository.Data)
            {
                categories.Add(new StatisticItem
                {
                    Category = category.Name,
                    Value = unitOfWork.PaymentRepository.Data
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