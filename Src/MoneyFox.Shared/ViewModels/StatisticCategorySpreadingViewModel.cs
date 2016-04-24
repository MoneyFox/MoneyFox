using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.StatisticDataProvider;
using OxyPlot;
using OxyPlot.Series;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticCategorySpreadingViewModel : StatisticViewModel
    {
        private readonly OxyColor[] colors =
        {
            OxyColor.Parse("#411718"), OxyColor.Parse("#5c2021"),
            OxyColor.Parse("#77292a"), OxyColor.Parse("#933233"),
            OxyColor.Parse("#af3a3c"), OxyColor.Parse("#c44a4c"),
            OxyColor.Parse("#ce6466")
        };

        private readonly CategorySpreadingDataProvider speadingDataProvider;

        public StatisticCategorySpreadingViewModel(IPaymentRepository paymentRepository,
            IRepository<Category> categoryRepository)
        {
            speadingDataProvider = new CategorySpreadingDataProvider(paymentRepository, categoryRepository);
        }

        /// <summary>
        ///     Contains the PlotModel for the CategorySpreading graph
        /// </summary>
        public PlotModel SpreadingModel { get; set; }

        public ObservableCollection<LegendItem> LegendList { get; set; }

        protected override void Load()
        {
            SpreadingModel = null;
            LegendList = new ObservableCollection<LegendItem>();
            SpreadingModel = GetSpreadingModel();
        }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        private PlotModel GetSpreadingModel()
        {
            var items = speadingDataProvider.GetValues(StartDate, EndDate);

            var statisticItems = items as IList<StatisticItem> ?? items.ToList();
            if (!statisticItems.Any())
            {
                return new PlotModel();
            }

            var model = new PlotModel();

            if (Settings.DarkThemeSelected)
            {
                model.Background = OxyColors.Black;
                model.TextColor = OxyColors.White;
            }
            else
            {
                model.Background = OxyColors.White;
                model.TextColor = OxyColors.Black;
            }

            var pieSeries = new PieSeries
            {
                InsideLabelFormat = ""
            };

            var colorIndex = 0;
            foreach (var item in statisticItems)
            {
                pieSeries.Slices.Add(new PieSlice(item.Label, item.Value) {Fill = colors[colorIndex]});
                LegendList.Add(new LegendItem {Color = colors[colorIndex], Text = item.Label});
                colorIndex ++;
            }

            model.Series.Add(pieSeries);
            return model;
        }
    }

    public class LegendItem
    {
        public OxyColor Color { get; set; }
        public string Text { get; set; }
    }
}