using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation.Interfaces.ViewModels;
using MoneyFox.Foundation.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace MoneyFox.Business.ViewModels
{
    public class StatisticCategorySpreadingViewModel : StatisticViewModel, IStatisticCategorySpreadingViewModel
    {
        private readonly CategorySpreadingDataProvider spreadingDataProvider;
        private ObservableCollection<LegendItem> legendList;

        public StatisticCategorySpreadingViewModel(CategorySpreadingDataProvider spreadingDataProvider,
            IMvxMessenger messenger) 
            : base(messenger)
        {
            this.spreadingDataProvider = spreadingDataProvider;
            StatisticItems = new MvxObservableCollection<StatisticItem>();
        }

        /// <summary>
        ///     A collection with the current items for the legend.
        /// </summary>
        public ObservableCollection<LegendItem> LegendList
        {
            get { return legendList; }
            set
            {
                if (legendList == value) return;
                legendList = value;
                RaisePropertyChanged();
            }
        }

        protected override void Load()
        {
            LegendList = new ObservableCollection<LegendItem>();
            LoadStatisticData();
        }

        public MvxObservableCollection<StatisticItem> StatisticItems { get; set; }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        private void LoadStatisticData()
        {
            var items = spreadingDataProvider.GetValues(StartDate, EndDate).ToList();
            StatisticItems.Clear();
            StatisticItems.AddRange(items);
        }
    }
}