using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.ViewModels
{
    public class StatisticCategorySpreadingViewModel : StatisticViewModel, IStatisticCategorySpreadingViewModel
    {
        private readonly CategorySpreadingDataProvider spreadingDataProvider;

        public StatisticCategorySpreadingViewModel(CategorySpreadingDataProvider spreadingDataProvider,
            IMvxMessenger messenger) 
            : base(messenger)
        {
            this.spreadingDataProvider = spreadingDataProvider;
            StatisticItems = new MvxObservableCollection<StatisticItem>();
        }

        protected override void Load()
        {
            LoadStatisticData();
        }

        public MvxObservableCollection<StatisticItem> StatisticItems { get; set; }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        private async void LoadStatisticData()
        {
            var items = await spreadingDataProvider.GetValues(StartDate, EndDate);
            StatisticItems.Clear();

            foreach (var statisticItem in items)
            {
                StatisticItems.Add(statisticItem);
            }
        }
    }
}