namespace MoneyFox.Ui.ViewModels.Statistics;

using System.Collections.ObjectModel;
using Core.ApplicationCore.Queries.Statistics;
using Core.Common.Extensions;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using MediatR;

internal sealed class StatisticCashFlowViewModel : StatisticViewModel
{
    public StatisticCashFlowViewModel(IMediator mediator) : base(mediator) { }

    public ObservableCollection<ISeries> Series { get; } = new();

    public List<ICartesianAxis> XAxis { get; } = new() { new Axis { IsVisible = false } };

    protected override async Task LoadAsync()
    {
        var statisticItems = await Mediator.Send(new GetCashFlowQuery { EndDate = EndDate, StartDate = StartDate });
        var cartesianItems = statisticItems.Select(
            x => new ColumnSeries<decimal>
            {
                Name = x.Label,
                TooltipLabelFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                Values = new List<decimal> { x.Value }
            });

        Series.Clear();
        Series.AddRange(cartesianItems);
    }
}
