using MediatR;
using Microcharts;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries.GetCashFlow;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Statistics
{
    /// <summary>
    /// Representation of the cash flow view.
    /// </summary>
    public class StatisticCashFlowViewModel : MobileStatisticViewModel
    {
        private const int BAR_CHART_MARGINS = 20;
        private const float BAR_CHART_TEXT_SIZE = 26f;

        private static readonly string? fontFamily = Device.RuntimePlatform == Device.iOS
                                                        ? "Lobster-Regular"
                                                        : null;

        private readonly SKTypeface typeFaceForIOS12 = SKTypeface.FromFamilyName(fontFamily);

        private BarChart chart = new BarChart();

        public StatisticCashFlowViewModel(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Chart to render.
        /// </summary>
        public BarChart Chart
        {
            get => chart;
            set
            {
                if(chart == value)
                {
                    return;
                }

                chart = value;
                RaisePropertyChanged();
            }
        }

        protected override async Task LoadAsync()
        {
            List<StatisticEntry>? statisticItems = await Mediator.Send(new GetCashFlowQuery
            {
                EndDate = EndDate,
                StartDate = StartDate
            });

            Chart = new BarChart
            {
                Entries = statisticItems.Select(x => new ChartEntry((float)x.Value)
                {
                    Label = x.Label,
                    ValueLabel = x.ValueLabel,
                    Color = SKColor.Parse(x.Color),
                    ValueLabelColor = SKColor.Parse(x.Color)
                }).ToList(),
                BackgroundColor = SKColors.Transparent,
                Margin = BAR_CHART_MARGINS,
                LabelTextSize = BAR_CHART_TEXT_SIZE,
                Typeface = typeFaceForIOS12
            };
        }
    }
}
