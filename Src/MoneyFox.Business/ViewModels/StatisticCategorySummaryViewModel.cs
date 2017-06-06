using System.Collections.ObjectModel;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;
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
            CategorySummary = new MvxObservableCollection<StatisticItem>();
        }

        /// <summary>
        ///     Returns the CategoryViewModel Summary
        /// </summary>
        public ObservableCollection<StatisticItem> CategorySummary { get; set; }

        protected override async void Load()
        {
            var items = await categorySummaryDataDataProvider.GetValues(StartDate, EndDate);
            CategorySummary.Clear();

            foreach (var statisticItem in items)
            {
                CategorySummary.Add(statisticItem);
            }
        }
    }
}