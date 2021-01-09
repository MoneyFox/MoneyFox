using MediatR;
using Microcharts;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries.GetCashFlow;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Statistics
{
    /// <summary>
    /// Representation of the cash flow view.
    /// </summary>
    public class StatisticCashFlowViewModel : StatisticViewModel
    {
        private BarChart chart = new BarChart();

        public StatisticCashFlowViewModel(IMediator mediator, IDialogService dialogService) : base(mediator, dialogService)
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
                BackgroundColor = ChartOptions.BackgroundColor,
                Margin = ChartOptions.Margin,
                LabelTextSize = ChartOptions.LabelTextSize,
                Typeface = ChartOptions.TypeFace
            };
        }
    }
}
