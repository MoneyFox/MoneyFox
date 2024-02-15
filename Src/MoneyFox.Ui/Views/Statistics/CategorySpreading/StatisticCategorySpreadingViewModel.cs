namespace MoneyFox.Ui.Views.Statistics.CategorySpreading;

using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Extensions;
using Core.Queries.Statistics;
using Domain.Aggregates.AccountAggregate;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MediatR;
using MoneyFox.Core.Common.Settings;
using Infrastructure.Adapters;

internal sealed class StatisticCategorySpreadingViewModel : StatisticViewModel
{
    private PaymentType selectedPaymentType;

    private readonly ISettingsFacade settingsFacade;

    public StatisticCategorySpreadingViewModel(IMediator mediator, IPopupService popupService) : base(mediator: mediator, popupService: popupService)
    {
        settingsFacade = new SettingsFacade(new SettingsAdapter());
    }

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

    public int NumberOfCategories
    {
        get => settingsFacade.DefaultNumberOfCategoriesInSpreading;

        set
        {
            if (settingsFacade.DefaultNumberOfCategoriesInSpreading == value) return;
            if (value < 1) value = 1;
            if (value > 15) value = 15; // 15 categories should be plenty

            settingsFacade.DefaultNumberOfCategoriesInSpreading = value;

            OnPropertyChanged();
            LoadAsync().GetAwaiter().GetResult();
        }
    }

    public RelayCommand IncreaseNumberOfCategories => new(
        () => UpdateNumberOfCategories(1, 1));

    public RelayCommand DecreaseNumberOfCategories => new(
        () => UpdateNumberOfCategories(1, -1));

    public void UpdateNumberOfCategories(int step, int index)
    {
        NumberOfCategories += step * index;
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
                paymentType: SelectedPaymentType,
                numberOfCategoriesToShow: NumberOfCategories));

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
