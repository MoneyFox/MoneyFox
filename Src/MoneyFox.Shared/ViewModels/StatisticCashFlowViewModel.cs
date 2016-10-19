using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.StatisticDataProvider;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MoneyFox.Shared.ViewModels
{
    public class StatisticCashFlowViewModel : StatisticViewModel
    {
        private readonly CashFlowDataProvider cashFlowDataProvider;
        private readonly ISettingsManager settingsManager;
        private PlotModel cashFlowModel;

        public StatisticCashFlowViewModel(IPaymentRepository paymentRepository, ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            cashFlowDataProvider = new CashFlowDataProvider(paymentRepository);
            CashFlowModel = GetCashFlowModel();
        }

        /// <summary>
        ///     Contains the PlotModel for the CashFlow graph
        /// </summary>
        public PlotModel CashFlowModel
        {
            get { return cashFlowModel; }
            set
            {
                cashFlowModel = value;
                RaisePropertyChanged();
            }
        }

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

            var model = new PlotModel();

            var columnSeries = new ColumnSeries();
            var axe = new CategoryAxis
            {
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Angle = 45
            };

            if (settingsManager.IsDarkThemeSelected)
            {
                axe.AxislineColor = OxyColors.White;
                axe.TextColor = OxyColors.White;

                model.Background = OxyColors.Black;
                model.TextColor = OxyColors.White;
            }
            else
            {
                axe.AxislineColor = OxyColors.Black;
                axe.AxislineColor = OxyColors.Black;

                model.Background = OxyColors.White;
                model.TextColor = OxyColors.Black;
            }

            columnSeries.Items.Add(new ColumnItem(cashFlow.Income.Value) {Color = OxyColors.LightGreen});
            axe.Labels.Add(cashFlow.Income.Label);
            columnSeries.Items.Add(new ColumnItem(cashFlow.Spending.Value) {Color = OxyColor.FromRgb(196, 54, 51)});
            axe.Labels.Add(cashFlow.Spending.Label);
            columnSeries.Items.Add(new ColumnItem(cashFlow.Revenue.Value) {Color = OxyColors.Cyan});
            axe.Labels.Add(cashFlow.Revenue.Label);

            model.Axes.Add(axe);
            model.Series.Add(columnSeries);
            return model;
        }
    }
}