namespace MoneyFox.ViewModels.Statistics
{
    using Core.Queries.Statistics;
    using Core.Queries.Statistics.Queries;
    using MediatR;
    using Microcharts;
    using SkiaSharp;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Views.Statistics;
    using Xamarin.Essentials;

    /// <summary>
    ///     Representation of the cash flow view.
    /// </summary>
    public class StatisticCashFlowViewModel : StatisticViewModel
    {
        private BarChart chart = new BarChart();

        public StatisticCashFlowViewModel(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        ///     Chart to render.
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
                OnPropertyChanged();
            }
        }

        protected override async Task LoadAsync()
        {
            List<StatisticEntry>? statisticItems =
                await Mediator.Send(new GetCashFlowQuery { EndDate = EndDate, StartDate = StartDate });

            Chart = new BarChart
            {
                Entries = statisticItems.Select(
                        x => new ChartEntry((float)x.Value)
                        {
                            Label = x.Label,
                            ValueLabel = x.ValueLabel,
                            Color = SKColor.Parse(x.Color),
                            ValueLabelColor = SKColor.Parse(x.Color)
                        })
                    .ToList(),
                BackgroundColor = new SKColor(ChartOptions.BackgroundColor.ToUInt()),
                Margin = ChartOptions.Margin,
                LabelTextSize = ChartOptions.LabelTextSize,
                Typeface = SKTypeface.FromFamilyName(ChartOptions.TypeFace)
            };
        }
    }
}