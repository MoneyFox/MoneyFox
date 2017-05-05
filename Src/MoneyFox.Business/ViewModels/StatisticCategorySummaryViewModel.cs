using System.Collections.ObjectModel;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.Models;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.ViewModels
{
    public class StatisticCategorySummaryViewModel : StatisticViewModel, IStatisticCategorySummaryViewModel
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

        protected override async void Load()
        {
            CategorySummary = null;
            CategorySummary = new ObservableCollection<StatisticItem>(await categorySummaryDataDataProvider.GetValues(StartDate, EndDate));
        }
    }
}