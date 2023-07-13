namespace MoneyFox.Ui.Views.Statistics.CashFlow;

using System.Collections.ObjectModel;
using Core.Queries.Statistics;
using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using MediatR;
using Resources.Strings;

internal sealed class StatisticCashFlowViewModel : StatisticViewModel
{
    public StatisticCashFlowViewModel(IMediator mediator) : base(mediator) { }

    public ObservableCollection<ISeries> Series { get; } = new();

    public List<ICartesianAxis> XAxis { get; } = new() { new Axis { IsVisible = false } };

    protected override async Task LoadAsync()
    {
        var cashFlowData = await Mediator.Send(new GetCashFlow.Query(startDate: DateOnly.FromDateTime(StartDate), endDate: DateOnly.FromDateTime(EndDate)));
        Series.Clear();
        Series.Add(
            new ColumnSeries<decimal>
            {
                Name = Translations.IncomeLabel,
                DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.Coordinate.PrimaryValue:C}",
                Values = new List<decimal> { cashFlowData.Income }
            });

        Series.Add(
            new ColumnSeries<decimal>
            {
                Name = Translations.ExpenseLabel,
                DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.Coordinate.PrimaryValue:C}",
                Values = new List<decimal> { cashFlowData.Expense }
            });

        Series.Add(
            new ColumnSeries<decimal>
            {
                Name = Translations.GainsLabel,
                DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.Coordinate.PrimaryValue:C}",
                Values = new List<decimal> { cashFlowData.Gain }
            });
    }
}
