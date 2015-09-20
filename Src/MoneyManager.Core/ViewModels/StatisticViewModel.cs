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

        public StatisticViewModel(StatisticManager statisticManager)
        {
            this.statisticManager = statisticManager;

            StartDate = DateTime.Now.Date.AddMonths(-1);
            EndDate = DateTime.Now.Date;
        }

        private PlotModel cashFlowModel;
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
                cashFlowModel = value;
                RaisePropertyChanged();
            }
        }

        private PlotModel spreadingModel;
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
                spreadingModel = value;
                RaisePropertyChanged();
            }
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

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
            }
        }

        public string Title => Strings.StatisticTitle + " " + StartDate.ToString("d") +
                               " - " +
                               EndDate.ToString("d");

        public void SetDefaultCashFlow()
        {
            SetCashFlowModel(statisticManager.GetMonthlyCashFlow());
        }

        public void SetDefaultSpreading()
        {
            SetSpreadingModel(statisticManager.GetSpreading());
        }

        private void SetSpreadingModel(IEnumerable<StatisticItem> items)
        {
            var model = new PlotModel
            {
                Background = OxyColors.Black,
                TextColor = OxyColors.White,
            };
            var pieSeries = new PieSeries();

            foreach (var item in items)
            {
                pieSeries.Slices.Add(new PieSlice(item.Label, item.Value));
            }

            model.IsLegendVisible = true;
            model.LegendPosition = LegendPosition.BottomLeft;

            model.Series.Add(pieSeries);
            SpreadingModel = model;
        }

        public void SetCustomCashFlow()
        {
            SetCashFlowModel(statisticManager.GetMonthlyCashFlow(StartDate, EndDate));
        }

        private void SetCashFlowModel(IEnumerable<StatisticItem> items)
        {
            var model = new PlotModel
            {
                Background = OxyColors.Black,
                TextColor = OxyColors.White,
            };
            var barSeries = new ColumnSeries();
            var axe = new CategoryAxis
            {
                AxislineColor = OxyColors.White,
                TextColor = OxyColors.White
            };

            foreach (var item in items)
            {
                barSeries.Items.Add(new ColumnItem(item.Value));
                axe.Labels.Add(item.Label);
            }

            barSeries.Items[0].Color = OxyColors.LightGreen;
            barSeries.Items[1].Color = OxyColors.Red;
            barSeries.Items[2].Color = OxyColors.Cyan;

            model.IsLegendVisible = true;
            model.LegendPosition = LegendPosition.BottomLeft;

            model.Axes.Add(axe);

            model.Series.Add(barSeries);
            CashFlowModel = model;
        }

        public void SetCustomSpreading()
        {
            SetSpreadingModel(statisticManager.GetSpreading(StartDate, EndDate));
        }

        public void SetCagtegorySummary()
        {
            CategorySummary = statisticManager.GetCategorySummary(StartDate, EndDate);
        }
    }
}