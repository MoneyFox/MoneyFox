using System.Collections.ObjectModel;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation.Models;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.ViewModels
{
    public class StatisticCategorySummaryViewModel : StatisticViewModel
    {
        private readonly CategorySummaryDataProvider categorySummaryDataDataProvider;
        private ObservableCollection<StatisticItem> categorySummary;

        public StatisticCategorySummaryViewModel(CategorySummaryDataProvider categorySummaryDataDataProvider,
            IMvxMessenger messenger) : base(messenger)
        {
            this.categorySummaryDataDataProvider = categorySummaryDataDataProvider;
        }

        /// <summary>
        ///     Returns the CategoryViewModel Summary
        /// </summary>
        public ObservableCollection<StatisticItem> CategorySummary
        {
            get { return categorySummary; }
            set
            {
                categorySummary = value;
                RaisePropertyChanged();
            }
        }

        protected override void Load()
        {
            CategorySummary = null;
            CategorySummary = GetCategorySummaryData();
        }

        private ObservableCollection<StatisticItem> GetCategorySummaryData()
            => new ObservableCollection<StatisticItem>(categorySummaryDataDataProvider.GetValues(StartDate, EndDate));
    }
}