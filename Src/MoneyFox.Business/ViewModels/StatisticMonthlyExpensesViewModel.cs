using System;
using MoneyFox.Business.Extensions;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Plugins.Messenger;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MoneyFox.Business.ViewModels
{
    public class StatisticMonthlyExpensesViewModel : StatisticViewModel
    {
        private readonly OxyColor expenseRed = OxyColor.Parse("#c43633");
        private readonly MonthlyExpensesDataProvider monthlyExpensesDataProvider;
        private readonly ISettingsManager settingsManager;
        private PlotModel monthlyExpensesModel;

        public StatisticMonthlyExpensesViewModel(MonthlyExpensesDataProvider monthlyExpensesDataProvider,
            ISettingsManager settingsManager,
            IMvxMessenger messenger)
            : base(DateTime.Today.AddMonths(-6), DateTime.Now.GetLastDayOfMonth(), messenger)
        {
            this.monthlyExpensesDataProvider = monthlyExpensesDataProvider;
            this.settingsManager = settingsManager;
            MonthlyExpensesModel = GetModel();
        }

        /// <summary>
        ///     Contains the PlotModel for the Chart
        /// </summary>
        public PlotModel MonthlyExpensesModel
        {
            get { return monthlyExpensesModel; }
            set
            {
                monthlyExpensesModel = value;
                RaisePropertyChanged();
            }
        }

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
            var monthlyExpenses = monthlyExpensesDataProvider.GetValues(StartDate, EndDate);

            //TODO: refactor this into an helper class
            var model = new PlotModel();

            var columnSeriesIncome = new ColumnSeries();
            var columnSeriesExpense = new ColumnSeries();
            var columnSeriesRevenue = new ColumnSeries();
            var axe = new CategoryAxis
            {
                IsPanEnabled = false,
                IsZoomEnabled = false,
                //Angle = 45
            };

            if (settingsManager.IsDarkThemeSelected)
            {
                model.Background = OxyColors.Black;
                model.TextColor = OxyColors.White;

                axe.AxislineColor = OxyColors.White;
                axe.TextColor = OxyColors.White;
            }
            else
            {
                model.Background = OxyColors.White;
                model.TextColor = OxyColors.Black;
                axe.AxislineColor = OxyColors.Black;
                axe.TextColor = OxyColors.Black;
            }

            foreach (var statisticItem in monthlyExpenses)
            {
                columnSeriesIncome.Items.Add(new ColumnItem(statisticItem.Income.Value) {Color = OxyColors.LightGreen });
                columnSeriesExpense.Items.Add(new ColumnItem(statisticItem.Expense.Value) {Color = expenseRed});
                columnSeriesRevenue.Items.Add(new ColumnItem(statisticItem.Revenue.Value) {Color = OxyColors.Cyan });
                axe.Labels.Add(statisticItem.CashFlowLabel);
            }

            model.Axes.Add(axe);
            model.Series.Add(columnSeriesIncome);
            model.Series.Add(columnSeriesExpense);
            model.Series.Add(columnSeriesRevenue);
            return model;
        }
    }
}