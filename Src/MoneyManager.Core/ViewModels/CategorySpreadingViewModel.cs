using System.Collections.Generic;
using System.Linq;
using MoneyManager.Core.StatisticProvider;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MvvmCross.Core.ViewModels;
using OxyPlot;
using OxyPlot.Series;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class CategorySpreadingViewModel : StatisticViewModel
    {
        private readonly CategorySpreadingDataProvider speadingDataProvider;

        public CategorySpreadingViewModel(IPaymentRepository paymentRepository, IRepository<Category> categoryRepository)
        {
            speadingDataProvider = new CategorySpreadingDataProvider(paymentRepository, categoryRepository);
        }

        /// <summary>
        ///     Loads the data with the current start and end date.
        /// </summary>
        public MvxCommand LoadCommand => new MvxCommand(Load);

        private void Load()
        {
            SpreadingModel = null;
            SpreadingModel = GetSpreadingModel();
        }

        /// <summary>
        ///     Contains the PlotModel for the CategorySpreading graph
        /// </summary>
        public PlotModel SpreadingModel { get; set; }

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
            return model;
        }
    }
}
