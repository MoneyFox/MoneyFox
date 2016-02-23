using System.Collections.Generic;
using System.Linq;
using MoneyManager.Core.StatisticDataProvider;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using OxyPlot;
using OxyPlot.Series;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticCategorySpreadingViewModel : StatisticViewModel
    {
        private readonly CategorySpreadingDataProvider speadingDataProvider;

        private readonly OxyColor[] colors =
        {
            OxyColor.Parse("#411718"), OxyColor.Parse("#5c2021"),
            OxyColor.Parse("#77292a"), OxyColor.Parse("#933233"),
            OxyColor.Parse("#af3a3c"), OxyColor.Parse("#c44a4c"),
            OxyColor.Parse("#ce6466")
        };

        public StatisticCategorySpreadingViewModel(IPaymentRepository paymentRepository,
            IRepository<Category> categoryRepository)
        {
            speadingDataProvider = new CategorySpreadingDataProvider(paymentRepository, categoryRepository);
        }

        /// <summary>
        ///     Contains the PlotModel for the CategorySpreading graph
        /// </summary>
        public PlotModel SpreadingModel { get; set; }

        protected override void Load()
        {
            SpreadingModel = null;
            SpreadingModel = GetSpreadingModel();
        }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        private PlotModel GetSpreadingModel()
        {
            var items = speadingDataProvider.GetValues(StartDate, EndDate);
            var totalAmount = items.Sum(x => x.Value);

            var statisticItems = items as IList<StatisticItem> ?? items.ToList();
            if (!statisticItems.Any())
            {
                return new PlotModel();
            }

            var model = new PlotModel
            {
                Background = OxyColors.Black,
                TextColor = OxyColors.White
            };
            var pieSeries = new PieSeries
            {
                AreInsideLabelsAngled = true,
                InsideLabelFormat = "{1}",
                OutsideLabelFormat = "{0}"
            };

            var colorIndex = 0;
            foreach (var item in statisticItems)
            {
                pieSeries.Slices.Add(new PieSlice(item.Label, item.Value) {Fill = colors[colorIndex]});
                colorIndex ++;
            }

            model.Series.Add(pieSeries);
            return model;
        }
    }
}