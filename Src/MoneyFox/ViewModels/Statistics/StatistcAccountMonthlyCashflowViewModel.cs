using MediatR;
using Microcharts;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries.GetCashFlow;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Statistics
{
    /// <summary>
    /// Representation of the cash flow view.
    /// </summary>
    public class StatistcAccountMonthlyCashflowViewModel : MobileStatisticViewModel
    {
        private const int BAR_CHART_MARGINS = 20;
        private const float BAR_CHART_TEXT_SIZE = 26f;

        private static readonly string? fontFamily = Device.RuntimePlatform == Device.iOS
                                                        ? "Lobster-Regular"
                                                        : null;

        private readonly SKTypeface typeFaceForIOS12 = SKTypeface.FromFamilyName(fontFamily);

        private BarChart chart = new BarChart();

        public StatistcAccountMonthlyCashflowViewModel(IMediator mediator) : base(mediator)
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

        public ObservableCollection<AccountViewModel> Accounts { get; private set; } = new ObservableCollection<AccountViewModel>();

        public AccountViewModel? SelectedAccount { get; set; }

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
                BackgroundColor = BackgroundColor,
                Margin = BAR_CHART_MARGINS,
                LabelTextSize = BAR_CHART_TEXT_SIZE,
                Typeface = typeFaceForIOS12
            };
        }
    }
}
