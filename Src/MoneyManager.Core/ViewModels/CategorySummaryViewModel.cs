using System.Collections.ObjectModel;
using MoneyManager.Core.StatisticProvider;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.ViewModels
{
    public class CategorySummaryViewModel : StatisticViewModel
    {
        private readonly CategorySummaryDataProvider categorySummaryDataDataProvider;

        private ObservableCollection<StatisticItem> categorySummary;

        public CategorySummaryViewModel(IPaymentRepository paymentRepository, IRepository<Category> categoryRepository)
        {
            categorySummaryDataDataProvider = new CategorySummaryDataProvider(paymentRepository, categoryRepository);
        }

        /// <summary>
        ///     Returns the Category Summary
        /// </summary>
        public ObservableCollection<StatisticItem> CategorySummary
        {
            get
            {
                if (categorySummary == null)
                {
                    SetCategorySummaryData();
                }
                return categorySummary;
            }
            private set
            {
                categorySummary = value;
                RaisePropertyChanged();
            }
        }

        public void SetCategorySummaryData()
        {
            categorySummary = new ObservableCollection<StatisticItem>(categorySummaryDataDataProvider.GetValues(StartDate, EndDate));
        }
    }
}
