using MediatR;
using Microcharts;
using MoneyFox.Application.Statistics.Queries.GetCashFlow;
using SkiaSharp;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Statistics
{
    /// <summary>
    /// Representation of the cash flow view.
    /// </summary>
    public class
        StatisticCashFlowViewModel : StatisticViewModel, IStatisticCashFlowViewModel
    {
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
                    return;
                chart = value;
                RaisePropertyChanged();
            }
        }

        protected override async Task LoadAsync()
        {
            var statisticItems = await Mediator.Send(new GetCashFlowQuery
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
                    Color = SKColor.Parse(x.Color)
                }).ToList(),
                BackgroundColor = BackgroundColor,
                Margin = 20,
                LabelTextSize = 26f,
                Typeface = typeFaceForIOS12
            };
        }
    }
}
