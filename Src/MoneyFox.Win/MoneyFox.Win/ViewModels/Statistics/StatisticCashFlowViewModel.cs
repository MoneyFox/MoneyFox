namespace MoneyFox.Win.ViewModels.Statistics;

using Core._Pending_.Common.Extensions;
using Core.Queries.Statistics;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using MediatR;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

public class StatisticCashFlowViewModel : StatisticViewModel
{
    public StatisticCashFlowViewModel(IMediator mediator) : base(mediator)
    {
    }

    public ObservableCollection<ISeries> Series { get; } = new();

    public List<ICartesianAxis> XAxis { get; } = new() {new Axis {IsVisible = false}};

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
                    Values = new List<decimal> {x.Value}
                });

        Series.Clear();
        Series.AddRange(cartesianItems);
    }
}