using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticViewModel : BaseViewModel
    {
        private readonly StatisticManager statisticManager;

        private ObservableCollection<StatisticItem> categorySummary;

        /// <summary>
        ///     Creates a StatisticViewModel Object.
        /// </summary>
        /// <param name="statisticManager">Instance of <see cref="StatisticManager" />/></param>
        public StatisticViewModel(StatisticManager statisticManager)
        {
            this.statisticManager = statisticManager;

            StartDate = DateTime.Now.Date.AddMonths(-1);
            EndDate = DateTime.Now.Date;
        }

        /// <summary>
        ///     Contains the PlotModel for the CashFlow graph
        /// </summary>
        public PlotModel CashFlowModel => GetDefaultCashFlow();

        /// <summary>
        ///     Contains the PlotModel for the CategorySpreading graph
        /// </summary>
        public PlotModel SpreadingModel => GetDefaultSpreading();


        /// <summary>
        ///     Startdate for a custom statistic
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     Enddate for a custom statistic
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        ///     Returns the Category Summary
        /// </summary>
        public ObservableCollection<StatisticItem> CategorySummary
        {
            get
            {
                return categorySummary == null || !categorySummary.Any()
                    ? statisticManager.GetCategorySummary(StartDate, EndDate)
                    : categorySummary;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                categorySummary = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Returns the title for the category view
        /// </summary>
        public string Title => Strings.StatisticTitle + " " + StartDate.ToString("d") +
                               " - " +
                               EndDate.ToString("d");

        /// <summary>
        ///     Set  a default CashFlowModel with the set Start and Enddate
        /// </summary>
        public PlotModel GetDefaultCashFlow()
        {
            return SetCashFlowModel(statisticManager.GetMonthlyCashFlow());
        }

        /// <summary>
        ///     Set  a default CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        public PlotModel GetDefaultSpreading()
        {
            return SetSpreadingModel(statisticManager.GetSpreading());
        }

        private PlotModel SetSpreadingModel(IEnumerable<StatisticItem> items)
        {
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
            var pieSeries = new PieSeries();

            foreach (var item in statisticItems)
            {
                pieSeries.Slices.Add(new PieSlice(item.Label, item.Value));
            }

            model.IsLegendVisible = true;
            model.LegendPosition = LegendPosition.BottomLeft;

            model.Series.Add(pieSeries);
            return model;
        }

        /// <summary>
        ///     Set a custom CashFlowModel with the set Start and Enddate
        /// </summary>
        public void SetCustomCashFlow()
        {
            SetCashFlowModel(statisticManager.GetMonthlyCashFlow(StartDate, EndDate));
        }

        private PlotModel SetCashFlowModel(IEnumerable<StatisticItem> items)
        {
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

            var columnSeries = new ColumnSeries();
            var axe = new CategoryAxis
            {
                AxislineColor = OxyColors.White,
                TextColor = OxyColors.White,
                IsPanEnabled = false,
                IsZoomEnabled = false
            };

            foreach (var item in statisticItems)
            {
                columnSeries.Items.Add(new ColumnItem(item.Value));
                axe.Labels.Add(item.Label);
            }

            columnSeries.Items[0].Color = OxyColors.LightGreen;
            columnSeries.Items[1].Color = OxyColors.Red;
            columnSeries.Items[2].Color = OxyColors.Cyan;

            model.Axes.Add(axe);
            model.Series.Add(columnSeries);
            return model;
        }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        public void SetCustomSpreading()
        {
            SetSpreadingModel(statisticManager.GetSpreading(StartDate, EndDate));
        }

        /// <summary>
        ///     Set a custom CategoryModel with the set Start and Enddate
        /// </summary>
        public void SetCagtegorySummary()
        {
            CategorySummary = statisticManager.GetCategorySummary(StartDate, EndDate);
        }
    }
}