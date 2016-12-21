using System;
using System.Linq;
using MoneyFox.Business.Extensions;
using MoneyFox.Business.StatisticDataProvider;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace MoneyFox.Business.ViewModels
{
    public class StatisticMonthlyCashFlowViewModel : StatisticViewModel
    {
        private readonly OxyColor expenseRed = OxyColor.Parse("#c43633");
        private readonly CashFlowDataProvider monthlyExpensesDataProvider;
        private readonly ISettingsManager settingsManager;
        private PlotModel monthlyExpensesModel;

        public StatisticMonthlyCashFlowViewModel(CashFlowDataProvider monthlyExpensesDataProvider,
            ISettingsManager settingsManager,
            IMvxMessenger messenger)
            : base(DateTime.Today.AddMonths(-5), DateTime.Now.GetLastDayOfMonth(), messenger)
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

        public MvxObservableCollection<LegendItem> LegendList { get; set; } = new MvxObservableCollection<LegendItem>();

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
            var monthlyExpenses = monthlyExpensesDataProvider.GetCashFlowList(StartDate, EndDate).ToList();

            //TODO: refactor this into an helper class
            var model = new PlotModel();
            LegendList.Clear();

            var columnSeriesIncome = new ColumnSeries();
            var columnSeriesExpense = new ColumnSeries();
            var columnSeriesRevenue = new ColumnSeries();
            var axe = new CategoryAxis
            {
                IsPanEnabled = false,
                IsZoomEnabled = false,
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
                axe.Labels.Add(statisticItem.Month);
            }
            LegendList.Add(new LegendItem {Text = Strings.LegendHeaderText });
            LegendList.AddRange(monthlyExpenses.Select(x => new LegendItem { Text = x.Label}));

            model.Axes.Add(axe);
            model.Series.Add(columnSeriesIncome);
            model.Series.Add(columnSeriesExpense);
            model.Series.Add(columnSeriesRevenue);
            return model;
        }
    }
}