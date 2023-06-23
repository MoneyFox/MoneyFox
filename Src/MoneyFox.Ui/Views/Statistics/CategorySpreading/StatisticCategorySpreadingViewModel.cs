namespace MoneyFox.Ui.Views.Statistics.CategorySpreading;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Extensions;
using Core.Queries.Statistics;
using Domain.Aggregates.AccountAggregate;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MediatR;

internal sealed class StatisticCategorySpreadingViewModel : StatisticViewModel
{
    private PaymentType selectedPaymentType;

    public StatisticCategorySpreadingViewModel(IMediator mediator) : base(mediator) { }

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
            LoadDataCommand.Execute(null);
        }
    }

    public AsyncRelayCommand LoadDataCommand => new(LoadAsync);

    protected override async Task LoadAsync()
    {
        var statisticEntries = await Mediator.Send(new GetCategorySpreadingQuery(startDate: StartDate, endDate: EndDate, paymentType: SelectedPaymentType));
        var pieSeries = statisticEntries.Select(
            x => new PieSeries<decimal>
            {
                Name = x.CategoryName,
                DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                Values = new List<decimal> { x.Value },
                InnerRadius = 150
            });

        Series.Clear();
        Series.AddRange(pieSeries);
    }
}
