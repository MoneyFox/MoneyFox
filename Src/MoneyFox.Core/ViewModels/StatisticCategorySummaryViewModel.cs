using System.Collections.ObjectModel;
using MoneyFox.Core.Model;
using MoneyFox.Core.ViewModels;
using MoneyFox.Foundation.Model;
using MoneyManager.Core.StatisticDataProvider;
using MoneyManager.Foundation.Interfaces;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticCategorySummaryViewModel : StatisticViewModel
    {
        private readonly CategorySummaryDataProvider categorySummaryDataDataProvider;

        public StatisticCategorySummaryViewModel(IPaymentRepository paymentRepository,
            IRepository<Category> categoryRepository)
        {
            categorySummaryDataDataProvider = new CategorySummaryDataProvider(paymentRepository, categoryRepository);
        }

        /// <summary>
        ///     Returns the Category Summary
        /// </summary>
        public ObservableCollection<StatisticItem> CategorySummary { get; set; }

        protected override void Load()
        {
            CategorySummary = null;
            CategorySummary = GetCategorySummaryData();
        }

        private ObservableCollection<StatisticItem> GetCategorySummaryData()
        {
            return new ObservableCollection<StatisticItem>(categorySummaryDataDataProvider.GetValues(StartDate, EndDate));
        }
    }
}