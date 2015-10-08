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

        private PlotModel cashFlowModel;

        /// <summary>
        ///     Contains the PlotModel for the CashFlow graph
        /// </summary>
        public PlotModel CashFlowModel
        {
            get
            {
                if (cashFlowModel == null)
                {
                    SetDefaultCashFlow();
                }

                return cashFlowModel;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                cashFlowModel = value;
                RaisePropertyChanged();
            }
        }

        private PlotModel spreadingModel;

        /// <summary>
        ///     Contains the PlotModel for the CategorySpreading graph
        /// </summary>
        public PlotModel SpreadingModel
        {
            get
            {
                if (spreadingModel == null)
                {
                    SetDefaultSpreading();
                }

                return spreadingModel;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                spreadingModel = value;
                RaisePropertyChanged();
            }
        }

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
        public void SetDefaultCashFlow()
        {
            SetCashFlowModel(statisticManager.GetMonthlyCashFlow());
        }

        /// <summary>
        ///     Set  a default CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        public void SetDefaultSpreading()
        {
            SetSpreadingModel(statisticManager.GetSpreading());
        }

        private void SetSpreadingModel(IEnumerable<StatisticItem> items)
        {
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
            var pieSeries = new PieSeries();

            foreach (var item in statisticItems)
            {
                pieSeries.Slices.Add(new PieSlice(item.Label, item.Value));
            }

            model.IsLegendVisible = true;
            model.LegendPosition = LegendPosition.BottomLeft;

            model.Series.Add(pieSeries);
            SpreadingModel = model;
        }

        /// <summary>
        ///     Set a custom CashFlowModel with the set Start and Enddate
        /// </summary>
        public void SetCustomCashFlow()
        {
            SetCashFlowModel(statisticManager.GetMonthlyCashFlow(StartDate, EndDate));
            RaisePropertyChanged(nameof(CashFlowModel));
        }

        private void SetCashFlowModel(IEnumerable<StatisticItem> items)
        {
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
            CashFlowModel = model;
        }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        public void SetCustomSpreading()
        {
            SetSpreadingModel(statisticManager.GetSpreading(StartDate, EndDate));
            RaisePropertyChanged(nameof(SpreadingModel));
        }

        /// <summary>
        ///     Set a custom CategoryModel with the set Start and Enddate
        /// </summary>
        public void SetCategorySummary()
        {
            CategorySummary = statisticManager.GetCategorySummary(StartDate, EndDate);
            RaisePropertyChanged(nameof(CategorySummary));
        }
    }
}