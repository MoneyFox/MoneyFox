using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microcharts;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using MoneyFox.Presentation.Facades;
using MoneyFox.ServiceLayer.Facades;
using SkiaSharp;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    /// <summary>
    ///     Representation of the category Spreading View
    /// </summary>
    public class StatisticCategorySpreadingViewModel : StatisticViewModel, IStatisticCategorySpreadingViewModel
    {
        private readonly ICategorySpreadingDataProvider spreadingDataProvider;
        private DonutChart chart;
        private ObservableCollection<StatisticEntry> statisticItems;

        public static readonly SKColor[] Colors =
        {
            SKColor.Parse("#266489"),
            SKColor.Parse("#68B9C0"),
            SKColor.Parse("#90D585"),
            SKColor.Parse("#F3C151"),
            SKColor.Parse("#F37F64"),
            SKColor.Parse("#424856"),
            SKColor.Parse("#8F97A4")
        };

        public StatisticCategorySpreadingViewModel(ICategorySpreadingDataProvider spreadingDataProvider,
                                                   ISettingsFacade settingsFacade) : base(settingsFacade)
        {
            this.spreadingDataProvider = spreadingDataProvider;
        }

        /// <summary>
        ///     Chart to render.
        /// </summary>
        public DonutChart Chart
        {
            get => chart;
            set
            {
                if (chart == value) return;
                chart = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Statistic items to display.
        /// </summary>
        public ObservableCollection<StatisticEntry> StatisticItems
        {
            get => statisticItems;
            private set
            {
                if (statisticItems == value) return;
                statisticItems = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        protected override async Task Load()
        {
            StatisticItems = new ObservableCollection<StatisticEntry>(await spreadingDataProvider.GetValues(StartDate, EndDate));

            var microChartItems = StatisticItems
                                  .Select(x => new Entry(x.Value) {Label = x.Label, ValueLabel = x.ValueLabel})
                                  .ToList();

            for (int i = 0; i < microChartItems.Count; i++)
            {
                microChartItems[i].Color = Colors[i];
            }

            Chart = new DonutChart
            {
                Entries = microChartItems,
                BackgroundColor = BackgroundColor,
                LabelTextSize = 26f
            };
        }
    }
}
