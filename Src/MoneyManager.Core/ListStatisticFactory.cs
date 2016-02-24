using System;
using System.Collections.Generic;
using MoneyManager.Core.StatisticDataProvider;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core
{
    public class ListStatisticFactory
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly IPaymentRepository paymentRepository;

        public ListStatisticFactory(IPaymentRepository paymentRepository,
            IRepository<Category> categoryRepository)
        {
            this.paymentRepository = paymentRepository;
            this.categoryRepository = categoryRepository;
        }

        /// <summary>
        ///     Creates an Statistic Data Provider for a statistic who uses lists. For example the Category Spreading Statistic
        /// </summary>
        /// <param name="type">Type of the statistic</param>
        /// <returns>Instance of the Statistic Data Provider.</returns>
        public IStatisticProvider<IEnumerable<StatisticItem>> CreateListProvider(ListStatisticType type)
        {
            switch (type)
            {
                case ListStatisticType.CategorySpreading:
                    return new CategorySpreadingDataProvider(paymentRepository, categoryRepository);

                case ListStatisticType.CategorySummary:
                    return new CategorySummaryDataProvider(paymentRepository, categoryRepository);

                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
}