using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Foundation.Models;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.StatisticDataProvider;
using OxyPlot;
using OxyPlot.Series;

namespace MoneyFox.Shared.ViewModels
{
    public class StatisticCategorySpreadingViewModel : StatisticViewModel
    {
        private readonly OxyColor[] colors =
        {
            OxyColor.Parse("#393939"), OxyColor.Parse("#4b4b4b"),
            OxyColor.Parse("#5d5d5d"), OxyColor.Parse("#a75538"),
            OxyColor.Parse("#c16342"), OxyColor.Parse("#cb7a5d"),
            OxyColor.Parse("#d49078")
        };

        private readonly ISettingsManager settingsManager;

        private readonly CategorySpreadingDataProvider spreadingDataProvider;
        private PlotModel spreadingModel;

        public StatisticCategorySpreadingViewModel(CategorySpreadingDataProvider spreadingDataProvider,
            ISettingsManager settingsManager)
        {
            this.spreadingDataProvider = spreadingDataProvider;
            this.settingsManager = settingsManager;
        }

        /// <summary>
        ///     Contains the PlotModel for the CategorySpreading graph
        /// </summary>
        public PlotModel SpreadingModel
        {
            get { return spreadingModel; }
            set
            {
                spreadingModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     A collection with the current items for the legend.
        /// </summary>
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
            var items = spreadingDataProvider.GetValues(StartDate, EndDate);

            var statisticItems = items as IList<StatisticItem> ?? items.ToList();
            if (!statisticItems.Any())
            {
                return new PlotModel();
            }

            var model = new PlotModel();

            if (settingsManager.IsDarkThemeSelected)
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
                colorIndex++;
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