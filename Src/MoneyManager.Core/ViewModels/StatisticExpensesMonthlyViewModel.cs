using MoneyManager.Core.StatisticDataProvider;
using MoneyManager.Foundation.Interfaces;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticExpensesMonthlyViewModel : StatisticViewModel
    {
        private readonly MonthlyExpensesDataProvider monthlyExpensesDataProvider;

        public StatisticExpensesMonthlyViewModel(IPaymentRepository paymentRepository)
        {
            monthlyExpensesDataProvider = new MonthlyExpensesDataProvider(paymentRepository);

            MonthlyExpensesModel = GetModel();
        }

        /// <summary>
        ///     Contains the PlotModel for the Chart
        /// </summary>
        public PlotModel MonthlyExpensesModel { get; set; }

        /// <summary>
        ///     Loads the expense history with the current start and end date.
        /// </summary>
        protected override void Load()
        {
            MonthlyExpensesModel = null;
            MonthlyExpensesModel = GetModel();
        }

        private PlotModel GetModel()
        {
            var cashFlow = monthlyExpensesDataProvider.GetValues(StartDate, EndDate);

            var model = new PlotModel
            {
                Background = OxyColors.Black,
                TextColor = OxyColors.White
            };

            var columnSeries = new ColumnSeries();
            var axe = new CategoryAxis
            {
                AxislineColor = OxyColors.White,
                TextColor = OxyColors.White,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Angle = 45
            };

            foreach (var statisticItem in cashFlow)
            {
                columnSeries.Items.Add(new ColumnItem(statisticItem.Value) {Color = OxyColors.Red});
                axe.Labels.Add(statisticItem.Label);
            }

            model.Axes.Add(axe);
            model.Series.Add(columnSeries);
            return model;
        }
    }
}