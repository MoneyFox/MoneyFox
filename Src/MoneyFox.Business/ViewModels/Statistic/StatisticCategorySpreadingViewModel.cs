using System.Linq;
using System.Threading.Tasks;
using Microcharts;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Plugins.Messenger;
using SkiaSharp;

namespace MoneyFox.Business.ViewModels.Statistic
{
    /// <summary>
    ///     Representation of the category Spreading View
    /// </summary>
    public class StatisticCategorySpreadingViewModel : StatisticViewModel, IStatisticCategorySpreadingViewModel
    {
        private readonly ISettingsManager settingsManager;
        private readonly CategorySpreadingDataProvider spreadingDataProvider;
        private DonutChart chart;

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

        /// <summary>
        ///     Contstructor
        /// </summary>
        public StatisticCategorySpreadingViewModel(CategorySpreadingDataProvider spreadingDataProvider,
                                                   IMvxMessenger messenger,
                                                   ISettingsManager settingsManager)
            : base(messenger, settingsManager)
        {
            this.spreadingDataProvider = spreadingDataProvider;
            this.settingsManager = settingsManager;
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

        public override async Task Initialize()
        {
            await Load();
        }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        protected override async Task Load()
        {
            var items = (await spreadingDataProvider.GetValues(StartDate, EndDate)).ToList();

            for (int i = 0; i < items.Count; i++)
            {
                items[i].Color = Colors[i];
            }

            Chart = new DonutChart
            {
                Entries = items,
                BackgroundColor = BackgroundColor,
                Margin = 20,
                LabelTextSize = 26f
            };
        }
    }
}