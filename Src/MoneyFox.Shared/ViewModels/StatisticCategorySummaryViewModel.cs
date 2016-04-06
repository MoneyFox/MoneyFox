using System.Collections.ObjectModel;
using MoneyManager.Core.StatisticDataProvider;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticCategorySummaryViewModel : StatisticViewModel
    {
        private readonly CategorySummaryDataProvider categorySummaryDataDataProvider;

        public StatisticCategorySummaryViewModel(IPaymentRepository paymentRepository, IRepository<Category> categoryRepository)
        {
            categorySummaryDataDataProvider = new CategorySummaryDataProvider(paymentRepository, categoryRepository);
        }

        protected override void Load()
        {
            CategorySummary = null;
            CategorySummary = GetCategorySummaryData();
        }

        /// <summary>
        ///     Returns the Category Summary
        /// </summary>
        public ObservableCollection<StatisticItem> CategorySummary { get; set; }

        private ObservableCollection<StatisticItem> GetCategorySummaryData()
        {
            return new ObservableCollection<StatisticItem>(categorySummaryDataDataProvider.GetValues(StartDate, EndDate));
        }
    }
}
