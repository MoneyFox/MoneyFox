namespace MoneyFox.Win.ViewModels.Statistics.StatisticCategorySummary;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Groups;
using CommunityToolkit.Mvvm.Input;
using Core.ApplicationCore.Queries;
using Core.ApplicationCore.Queries.Statistics.GetCategorySummary;
using MediatR;
using Payments;

internal sealed class StatisticCategorySummaryViewModel : StatisticViewModel, IStatisticCategorySummaryViewModel
{
    private readonly IMapper mapper;
    private ObservableCollection<CategoryOverviewViewModel> categorySummary = new();

    private IncomeExpenseBalanceViewModel incomeExpenseBalance = new();

    private CategoryOverviewViewModel? selectedOverviewItem;

    public StatisticCategorySummaryViewModel(IMediator mediator, IMapper mapper) : base(mediator)
    {
        this.mapper = mapper;
    }

    public CategoryOverviewViewModel? SelectedOverviewItem
    {
        get => selectedOverviewItem;

        set
        {
            selectedOverviewItem = value;
            OnPropertyChanged();
        }
    }

    public IncomeExpenseBalanceViewModel IncomeExpenseBalance
    {
        get => incomeExpenseBalance;

        set
        {
            if (incomeExpenseBalance == value)
            {
                return;
            }

            incomeExpenseBalance = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<CategoryOverviewViewModel> CategorySummary
    {
        get => categorySummary;

        private set
        {
            if (categorySummary == value)
            {
                return;
            }

            categorySummary = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasData));
        }
    }

    public bool HasData => CategorySummary.Any();

    public RelayCommand<CategoryOverviewViewModel> SummaryEntrySelectedCommand => new(async c => await SummaryEntrySelectedAsync(c));

    private async Task SummaryEntrySelectedAsync(CategoryOverviewViewModel summaryItem)
    {
        var loadedPayments = mapper.Map<List<PaymentViewModel>>(
            await Mediator.Send(new GetPaymentsForCategorySummary.Query(categoryId: summaryItem.CategoryId, dateRangeFrom: StartDate, dateRangeTo: EndDate)));

        var dailyItems = DateListGroupCollection<PaymentViewModel>.CreateGroups(
            items: loadedPayments,
            getKey: s => s.Date.ToString(format: "D", provider: CultureInfo.CurrentCulture),
            getSortKey: s => s.Date);

        summaryItem.Source.Clear();
        DateListGroupCollection<DateListGroupCollection<PaymentViewModel>>.CreateGroups(
                items: dailyItems,
                getKey: s =>
                {
                    var date = Convert.ToDateTime(value: s.Key, provider: CultureInfo.CurrentCulture);

                    return $"{date.ToString(format: "MMMM", provider: CultureInfo.CurrentCulture)} {date.Year}";
                },
                getSortKey: s => Convert.ToDateTime(value: s.Key, provider: CultureInfo.CurrentCulture))
            .ForEach(summaryItem.Source.Add);

        SelectedOverviewItem = summaryItem;
    }

    /// <summary>
    ///     Overrides the load method to load the category summary data.
    /// </summary>
    protected override async Task LoadAsync()
    {
        var categorySummaryModel = await Mediator.Send(new GetCategorySummaryQuery { EndDate = EndDate, StartDate = StartDate });
        CategorySummary.Clear();
        categorySummaryModel.CategoryOverviewItems.Select(
                x => new CategoryOverviewViewModel
                {
                    CategoryId = x.CategoryId,
                    Value = x.Value,
                    Average = x.Average,
                    Label = x.Label,
                    Percentage = x.Percentage
                })
            .ToList()
            .ForEach(CategorySummary.Add);

        IncomeExpenseBalance.TotalEarned = categorySummaryModel.TotalEarned;
        IncomeExpenseBalance.TotalSpent = categorySummaryModel.TotalSpent;
    }
}
