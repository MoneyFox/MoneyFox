using MediatR;
using Microcharts;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using MoneyFox.Ui.Shared.ViewModels.Statistics;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Statistics
{
    /// <summary>
    /// Representation of the category Spreading View
    /// </summary>
    public class StatisticCategorySpreadingViewModel : StatisticViewModel
    {
        private DonutChart chart = new DonutChart();

        public StatisticCategorySpreadingViewModel(IMediator mediator, IDialogService dialogService) : base(mediator)
        {
        }

        /// <summary>
        /// Chart to render.
        /// </summary>
        public DonutChart Chart
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

        /// <summary>
        /// Set a custom CategorySpreadingModel with the set Start and End date
        /// </summary>
        protected override async Task LoadAsync()
        {
            var statisticItems = new ObservableCollection<StatisticEntry>(await Mediator.Send(new GetCategorySpreadingQuery(StartDate, EndDate)));

            var microChartItems = statisticItems.Select(x => new ChartEntry((float)x.Value)
            {
                Label = x.Label,
                ValueLabel = x.ValueLabel,
                Color = SKColor.Parse(x.Color),
                ValueLabelColor = SKColor.Parse(x.Color)
            })
            .ToList();

            Chart = new DonutChart
            {
                Entries = microChartItems,
                BackgroundColor = ChartOptions.BackgroundColor,
                Margin = ChartOptions.Margin,
                LabelTextSize = ChartOptions.LabelTextSize,
                Typeface = ChartOptions.TypeFace
            };
        }
    }
}
