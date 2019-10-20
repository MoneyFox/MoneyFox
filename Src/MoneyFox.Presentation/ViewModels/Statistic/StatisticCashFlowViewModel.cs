using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microcharts;
using MoneyFox.Application.Statistics.Models;
using MoneyFox.Application.Statistics.Queries.GetCashFlow;
using MoneyFox.Presentation.Facades;
using SkiaSharp;

namespace MoneyFox.Presentation.ViewModels.Statistic
{
    /// <summary>
    ///     Representation of the cash flow view.
    /// </summary>
    public class
        StatisticCashFlowViewModel : StatisticViewModel, IStatisticCashFlowViewModel
    {
        private BarChart chart;
        private ObservableCollection<StatisticEntry> statisticItems;

        public StatisticCashFlowViewModel(IMediator mediator,
                                          ISettingsFacade settingsFacade)
            : base(mediator, settingsFacade)
        {
            Chart = new BarChart();
        }

        /// <summary>
        ///     Chart to render.
        /// </summary>
        public BarChart Chart
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

        protected override async Task Load()
        {
            StatisticItems = new ObservableCollection<StatisticEntry>(await Mediator.Send(new GetCashFlowQuery
            {
                EndDate = EndDate,
                StartDate = StartDate
            }));

            Chart = new BarChart
            {
                Entries = StatisticItems.Select(x => new Entry(x.Value)
                                        {
                                            Label = x.Label,
                                            ValueLabel = x.ValueLabel,
                                            Color = SKColor.Parse(x.Color)
                                        })
                                        .ToList(),
                BackgroundColor = BackgroundColor,
                Margin = 20,
                LabelTextSize = 26f
            };
        }
    }
}
