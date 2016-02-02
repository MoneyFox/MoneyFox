using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Core.Extensions;
using MoneyManager.Core.StatisticProvider;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels.Statistics
{
    [ImplementPropertyChanged]
    public class StatisticViewModel : BaseViewModel
    {
        private readonly ListStatisticFactory listStatisticFactory;
        private readonly IPaymentRepository paymentRepository;

        private PlotModel cashFlowModel;

        private ObservableCollection<StatisticItem> categorySummary;

        private PlotModel spreadingModel;

        /// <summary>
        ///     Creates a StatisticViewModel Object.
        /// </summary>
        /// <param name="paymentRepository">Instance of <see cref="IPaymentRepository" /></param>
        /// <param name="categoryRepository">Instance of <see cref="IRepository{T}" /></param>
        public StatisticViewModel(IPaymentRepository paymentRepository, IRepository<Category> categoryRepository)
        {
            this.paymentRepository = paymentRepository;

            listStatisticFactory = new ListStatisticFactory(paymentRepository, categoryRepository);

            StartDate = DateTime.Today.GetFirstDayOfMonth();
            EndDate = DateTime.Today.GetLastDayOfMonth();
        }

        /// <summary>
        ///     Contains the PlotModel for the CashFlow graph
        /// </summary>
        public PlotModel CashFlowModel
        {
            get
            {
                if (cashFlowModel == null)
                {
                    SetCashFlow();
                }

                return cashFlowModel;
            }
            private set
            {
                cashFlowModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Contains the PlotModel for the CategorySpreading graph
        /// </summary>
        public PlotModel SpreadingModel
        {
            get
            {
                if (spreadingModel == null)
                {
                    SetSpreading();
                }

                return spreadingModel;
            }
            private set
            {
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
                    ? new ObservableCollection<StatisticItem>
                        (listStatisticFactory.CreateListProvider(ListStatisticType.CategorySummary)
                            .GetValues(StartDate, EndDate))
                    : categorySummary;
            }
            private set
            {
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
        ///     Set a custom CashFlowModel with the set Start and Enddate
        /// </summary>
        public void SetCashFlow()
        {
            CashFlowModel = null;
            SetCashFlowModel(new CashFlowProvider(paymentRepository).GetValues(StartDate, EndDate));
            RaisePropertyChanged(nameof(CashFlowModel));
        }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and Enddate
        /// </summary>
        public void SetSpreading()
        {
            SpreadingModel = null;
            SetSpreadingModel(
                listStatisticFactory.CreateListProvider(ListStatisticType.CategorySpreading)
                    .GetValues(StartDate, EndDate));
            RaisePropertyChanged(nameof(SpreadingModel));
        }

        /// <summary>
        ///     Set a custom CategoryModel with the set Start and Enddate
        /// </summary>
        public void SetCustomCategorySummary()
        {
            SetSpreadingModel(
                listStatisticFactory.CreateListProvider(ListStatisticType.CategorySummary).GetValues(StartDate, EndDate));
            RaisePropertyChanged(nameof(CategorySummary));
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
                TextColor = OxyColors.White,
                IsLegendVisible = true
            };
            var pieSeries = new PieSeries
            {
                AreInsideLabelsAngled = true,
                InsideLabelFormat = "{1}",
                OutsideLabelFormat = "{0}"
            };

            foreach (var item in statisticItems)
            {
                pieSeries.Slices.Add(new PieSlice(item.Label, item.Value));
            }

            model.Series.Add(pieSeries);
            SpreadingModel = model;
        }

        private void SetCashFlowModel(CashFlow cashFlow)
        {
            var model = new PlotModel
            {
                Background = OxyColors.Black,
                TextColor = OxyColors.White,
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
            CashFlowModel = model;
        }
    }
}