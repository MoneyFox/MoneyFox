using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MediatR;
using MoneyFox.Application.Statistics;
using MoneyFox.Application.Statistics.Queries;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Uno.Extensions;

namespace MoneyFox.Uwp.ViewModels.Statistics
{
    /// <summary>
    ///     Representation of the cash flow view.
    /// </summary>
    public class StatisticCashFlowViewModel : StatisticViewModel
    {
        public StatisticCashFlowViewModel(IMediator mediator) : base(mediator)
        {
        }

        public ObservableCollection<ISeries> Series { get; } = new ObservableCollection<ISeries>();

        protected override async Task LoadAsync()
        {
            List<StatisticEntry> statisticItems =
                await Mediator.Send(new GetCashFlowQuery {EndDate = EndDate, StartDate = StartDate});

            IEnumerable<ColumnSeries<decimal>> cartesianItems = statisticItems.Select(
                x =>
                    new ColumnSeries<decimal>
                    {
                        Name = x.Label,
                        TooltipLabelFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                        DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                        Values = new List<decimal> {x.Value},
                    });

            Series.Clear();
            Series.AddRange(cartesianItems);
        }
    }
}