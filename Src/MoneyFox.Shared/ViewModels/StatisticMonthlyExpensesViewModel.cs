using System;
using MoneyFox.Shared.Extensions;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.StatisticDataProvider;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticMonthlyExpensesViewModel : StatisticViewModel
    {
        private readonly OxyColor graphColor = OxyColor.Parse("#c43633");
        private readonly MonthlyExpensesDataProvider monthlyExpensesDataProvider;

        public StatisticMonthlyExpensesViewModel(IUnitOfWork unitOfWork)
            : base(DateTime.Today.AddMonths(-6), DateTime.Now.GetLastDayOfMonth())
        {
            monthlyExpensesDataProvider = new MonthlyExpensesDataProvider(unitOfWork);

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
            var monthlyExpenses = monthlyExpensesDataProvider.GetValues(StartDate, EndDate);

            //TODO: refactor this into an helper class
            var model = new PlotModel();

            var columnSeries = new ColumnSeries();
            var axe = new CategoryAxis
            {
                IsPanEnabled = false,
                IsZoomEnabled = false,
                Angle = 45
            };

            if (SettingsHelper.IsDarkThemeSelected)
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
                columnSeries.Items.Add(new ColumnItem(statisticItem.Value) {Color = graphColor});
                axe.Labels.Add(statisticItem.Label);
            }

            model.Axes.Add(axe);
            model.Series.Add(columnSeries);
            return model;
        }
    }
}