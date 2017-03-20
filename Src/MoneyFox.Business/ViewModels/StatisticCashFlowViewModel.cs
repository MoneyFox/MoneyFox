using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Models;
using MvvmCross.Plugins.Messenger;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MoneyFox.Business.ViewModels
{
    public class StatisticCashFlowViewModel : StatisticViewModel
    {
        private readonly OxyColor expenseRed = OxyColor.Parse("#c43633");

        private readonly CashFlowDataProvider cashFlowDataProvider;
        private readonly ISettingsManager settingsManager;
        private PlotModel cashFlowModel;

        public StatisticCashFlowViewModel(IPaymentRepository paymentRepository, 
            ISettingsManager settingsManager,
            IMvxMessenger messenger, CashFlowDataProvider cashFlowDataProvider) 
            : base(messenger)
        {
            this.settingsManager = settingsManager;
            this.cashFlowDataProvider = cashFlowDataProvider;
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

        public CashFlow CashFlow { get; set; }

        /// <summary>
        ///     Set a custom CashFlowModel with the set Start and Enddate
        /// </summary>
        public PlotModel GetCashFlowModel()
        {
            var cashFlow = cashFlowDataProvider.GetCashFlow(StartDate, EndDate);

            CashFlow = cashFlow;

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
            columnSeries.Items.Add(new ColumnItem(cashFlow.Expense.Value) {Color = expenseRed });
            axe.Labels.Add(cashFlow.Expense.Label);
            columnSeries.Items.Add(new ColumnItem(cashFlow.Revenue.Value) {Color = OxyColors.Cyan});
            axe.Labels.Add(cashFlow.Revenue.Label);

            model.Axes.Add(axe);
            model.Series.Add(columnSeries);
            return model;
        }
    }
}