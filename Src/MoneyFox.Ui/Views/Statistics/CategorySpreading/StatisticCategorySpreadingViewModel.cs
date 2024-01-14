namespace MoneyFox.Ui.Views.Statistics.CategorySpreading;

using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;
using Core.Common.Extensions;
using Core.Queries.Statistics;
using Domain.Aggregates.AccountAggregate;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MediatR;

internal sealed class StatisticCategorySpreadingViewModel : StatisticViewModel
{
    private PaymentType selectedPaymentType;

    public StatisticCategorySpreadingViewModel(IMediator mediator, IPopupService popupService) : base(mediator: mediator, popupService: popupService) { }

    public List<PaymentType> PaymentTypes => new() { PaymentType.Expense, PaymentType.Income };

    public ObservableCollection<ISeries> Series { get; } = new();

    public PaymentType SelectedPaymentType
    {
        get => selectedPaymentType;

        set
        {
            if (selectedPaymentType == value)
            {
                return;
            }

            selectedPaymentType = value;
            OnPropertyChanged();
            LoadAsync().GetAwaiter().GetResult();
        }
    }

    public override Task OnNavigatedAsync(object? parameter)
    {
        return LoadAsync();
    }

    protected override async Task LoadAsync()
    {
        var statisticEntries = await Mediator.Send(
            new GetCategorySpreading.Query(
                startDate: DateOnly.FromDateTime(StartDate),
                endDate: DateOnly.FromDateTime(EndDate),
                paymentType: SelectedPaymentType));

        var pieSeries = statisticEntries.Select(
            x => new PieSeries<decimal>
            {
                Name = x.CategoryName,
                DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.Coordinate.PrimaryValue:C}",
                Values = new List<decimal> { x.Value },
                InnerRadius = 50
            });

        Series.Clear();
        Series.AddRange(pieSeries);
    }
}
