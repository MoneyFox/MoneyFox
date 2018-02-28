using System.Threading.Tasks;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Representation of the category Spreading View
    /// </summary>
    public class StatisticCategorySpreadingViewModel : StatisticViewModel, IStatisticCategorySpreadingViewModel
    {
        private readonly CategorySpreadingDataProvider spreadingDataProvider;

        /// <summary>
        ///     Contstructor
        /// </summary>
        public StatisticCategorySpreadingViewModel(CategorySpreadingDataProvider spreadingDataProvider,
            IMvxMessenger messenger) 
            : base(messenger)
        {
            this.spreadingDataProvider = spreadingDataProvider;
            StatisticItems = new MvxObservableCollection<StatisticItem>();
        }

        /// <summary>
        ///     Called when loading of the view is completed.
        /// </summary>
        protected override async Task Load()
        {
            await LoadStatisticData();
        }

        /// <summary>
        ///     Statistic items to display.
        /// </summary>
        public MvxObservableCollection<StatisticItem> StatisticItems { get; set; }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        private async Task LoadStatisticData()
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