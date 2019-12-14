using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microcharts;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Statistics.Models;
using MoneyFox.Application.Statistics.Queries.GetCategorySpreading;
using SkiaSharp;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    /// <summary>
    ///     Representation of the category Spreading View
    /// </summary>
    public class StatisticCategorySpreadingViewModel : StatisticViewModel, IStatisticCategorySpreadingViewModel
    {
        private DonutChart chart;
        private ObservableCollection<StatisticEntry> statisticItems;

        public StatisticCategorySpreadingViewModel(IMediator mediator,
                                                   ISettingsFacade settingsFacade) : base(mediator, settingsFacade)
        {
        }

        /// <summary>
        ///     Chart to render.
        /// </summary>
        public DonutChart Chart
        {
            get => chart;
            set
            {
                if (chart == value) return;
                chart = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Statistic items to display.
        /// </summary>
        public ObservableCollection<StatisticEntry> StatisticItems
        {
            get => statisticItems;
            private set
            {
                if (statisticItems == value) return;
                statisticItems = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Set a custom CategprySpreadingModel with the set Start and End date
        /// </summary>
        protected override async Task Load()
        {
            StatisticItems =
                new ObservableCollection<StatisticEntry>(await Mediator.Send(new GetCategorySpreadingQuery {StartDate = StartDate, EndDate = EndDate}));

            List<Entry> microChartItems = StatisticItems
                                          .Select(x => new Entry(x.Value)
                                          {
                                              Label = x.Label,
                                              ValueLabel = x.ValueLabel,
                                              Color = SKColor.Parse(x.Color)
                                          })
                                          .ToList();

            Chart = new DonutChart
            {
                Entries = microChartItems,
                BackgroundColor = BackgroundColor,
                LabelTextSize = 26f
            };
        }
    }
}
