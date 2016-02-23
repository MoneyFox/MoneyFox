using MoneyManager.Core.StatisticDataProvider;
using MoneyManager.Foundation.Interfaces;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticCashFlowViewModel : StatisticViewModel
    {
        private readonly CashFlowDataProvider cashFlowDataProvider;

        public StatisticCashFlowViewModel(IPaymentRepository paymentRepository)
        {
            cashFlowDataProvider = new CashFlowDataProvider(paymentRepository);

            CashFlowModel = GetCashFlowModel();
        }

        /// <summary>
        ///     Contains the PlotModel for the CashFlow graph
        /// </summary>
        public PlotModel CashFlowModel { get; set; }

        /// <summary>
        ///     Loads the cashflow with the current start and end date.
        /// </summary>
        protected override void Load()
        {
            CashFlowModel = null;
            CashFlowModel = GetCashFlowModel();
        }

        /// <summary>
        ///     Set a custom CashFlowModel with the set Start and Enddate
        /// </summary>
        public PlotModel GetCashFlowModel()
        {
            var cashFlow = cashFlowDataProvider.GetValues(StartDate, EndDate);

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
                Angle = 45
            };

            columnSeries.Items.Add(new ColumnItem(cashFlow.Income.Value));
            axe.Labels.Add(cashFlow.Income.Label);
            columnSeries.Items.Add(new ColumnItem(cashFlow.Spending.Value));
            axe.Labels.Add(cashFlow.Spending.Label);
            columnSeries.Items.Add(new ColumnItem(cashFlow.Revenue.Value));
            axe.Labels.Add(cashFlow.Revenue.Label);

            columnSeries.Items[0].Color = OxyColors.LightGreen;
            columnSeries.Items[1].Color = OxyColors.Red;
            columnSeries.Items[2].Color = OxyColors.Cyan;

            model.Axes.Add(axe);
            model.Series.Add(columnSeries);
            return model;
        }
    }
}