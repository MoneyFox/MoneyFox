using System.Collections.Generic;
using System.Linq;
using MoneyManager.Core.StatisticProvider;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using OxyPlot;
using OxyPlot.Series;

namespace MoneyManager.Core.ViewModels
{
    public class SpreadingViewModel : StatisticViewModel
    {
        private readonly CategorySpreadingDataProvider speadingDataProvider;

        private PlotModel spreadingModel;

        public SpreadingViewModel(IPaymentRepository paymentRepository, IRepository<Category> categoryRepository)
        {
            speadingDataProvider = new CategorySpreadingDataProvider(paymentRepository, categoryRepository);
        }

        /// <summary>
        ///     Contains the PlotModel for the CategorySpreading graph
        /// </summary>
        public PlotModel SpreadingModel
        {
            get
            {
                if (spreadingModel == null)
                {
                    SetSpreadingModel();
                }

                return spreadingModel;
            }
            private set
            {
                spreadingModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        public void SetSpreadingModel()
        {
            SpreadingModel = null;
            var items = speadingDataProvider.GetValues(StartDate, EndDate);

            var statisticItems = items as IList<StatisticItem> ?? items.ToList();
            if (!statisticItems.Any())
            {
                return;
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

            foreach (var item in statisticItems)
            {
                pieSeries.Slices.Add(new PieSlice(item.Label, item.Value));
            }

            model.Series.Add(pieSeries);
            SpreadingModel = model;
        }
    }
}
