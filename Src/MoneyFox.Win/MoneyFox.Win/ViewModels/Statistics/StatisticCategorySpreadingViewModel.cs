namespace MoneyFox.Win.ViewModels.Statistics;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Core._Pending_.Common.Extensions;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries.Statistics;
using Core.Common.Facades;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MediatR;

/// <summary>
///     Representation of the category Spreading View
/// </summary>
public class StatisticCategorySpreadingViewModel : StatisticViewModel
{
    private readonly ISettingsFacade settingsFacade;
    private PaymentType selectedPaymentType;

    public StatisticCategorySpreadingViewModel(IMediator mediator, ISettingsFacade settingsFacade) : base(mediator)
    {
        this.settingsFacade = settingsFacade;
    }

    public List<PaymentType> PaymentTypes => new() { PaymentType.Expense, PaymentType.Income };

    public PaymentType SelectedPaymentType
    {
        get => selectedPaymentType;
        set => SetProperty(field: ref selectedPaymentType, newValue: value);
    }

    public ObservableCollection<ISeries> Series { get; } = new();

    /// <summary>
    ///     Amount of categories to show. All Payments not fitting in here will go to the other category
    /// </summary>
    public int NumberOfCategoriesToShow
    {
        get => settingsFacade.CategorySpreadingNumber;

        set
        {
            if (settingsFacade.CategorySpreadingNumber == value)
            {
                return;
            }

            settingsFacade.CategorySpreadingNumber = value;
            OnPropertyChanged();
        }
    }

    public RelayCommand LoadDataCommand => new(async () => await LoadAsync());

    /// <summary>
    ///     Set a custom CategorySpreadingModel with the set Start and End date
    /// </summary>
    protected override async Task LoadAsync()
    {
        var statisticEntries = await Mediator.Send(
            new GetCategorySpreadingQuery(
                startDate: StartDate,
                endDate: EndDate,
                paymentType: SelectedPaymentType,
                numberOfCategoriesToShow: NumberOfCategoriesToShow));

        var pieSeries = statisticEntries.Select(
            x => new PieSeries<decimal>
            {
                Name = x.Label,
                TooltipLabelFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.PrimaryValue:C}",
                Values = new List<decimal> { x.Value },
                InnerRadius = 150
            });

        Series.Clear();
        Series.AddRange(pieSeries);
    }
}
