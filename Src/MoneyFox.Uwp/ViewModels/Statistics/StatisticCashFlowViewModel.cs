using MediatR;
using Microcharts;
using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core.Queries.Statistics;
using MoneyFox.Core.Queries.Statistics.Queries;
using MoneyFox.Uwp.Views.Statistics;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels.Statistics
{
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
                BackgroundColor = new SKColor(
                    ChartOptions.BackgroundColor.R,
                    ChartOptions.BackgroundColor.G,
                    ChartOptions.BackgroundColor.B,
                    ChartOptions.BackgroundColor.A),
                Margin = ChartOptions.Margin,
                LabelTextSize = ChartOptions.LabelTextSize,
                Typeface = SKTypeface.FromFamilyName(ChartOptions.TypeFace)
            };
        }
    }
}