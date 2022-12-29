namespace MoneyFox.Ui.ViewModels.Statistics;

using System.Collections.ObjectModel;
using Core.ApplicationCore.Queries.Statistics;
using Core.Common.Extensions;
using Core.Resources;
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
        var cashFlowData = await Mediator.Send(new GetCashFlowQuery { EndDate = EndDate, StartDate = StartDate });

        Series.Clear();
        Series.Add(new ColumnSeries<decimal>
        {
            Name = Translations.IncomeLabel,
            TooltipLabelFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
            DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
            Values = new List<decimal> { cashFlowData.Income }
        });
        Series.Add(new ColumnSeries<decimal>
        {
            Name = Translations.ExpenseLabel,
            TooltipLabelFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
            DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
            Values = new List<decimal> { cashFlowData.Expense }
        });
        Series.Add(new ColumnSeries<decimal>
        {
            Name = Translations.GainsLabel,
            TooltipLabelFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
            DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
            Values = new List<decimal> { cashFlowData.Gain }
        });
    }
}
