namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using System.Collections.ObjectModel;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Queries.Statistics.GetCategorySummary;
using MediatR;
using Serilog;

internal sealed class StatisticCategorySummaryViewModel : StatisticViewModel
{
    private readonly IDialogService dialogService;

    private ObservableCollection<CategoryOverviewViewModel> categorySummary = new();
    private decimal totalExpense;
    private decimal totalIncome;

    public StatisticCategorySummaryViewModel(IMediator mediator, IDialogService dialogService) : base(mediator)
    {
        this.dialogService = dialogService;
        CategorySummary = new();
    }

    public ObservableCollection<CategoryOverviewViewModel> CategorySummary
    {
        get => categorySummary;

        set
        {
            categorySummary = value;
            TotalExpense = Math.Abs(categorySummary.Where(x => x.Value < 0).Sum(x => x.Value));
            TotalIncome = categorySummary.Where(x => x.Value > 0).Sum(x => x.Value);
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasData));
        }
    }

    public decimal TotalExpense
    {
        get => totalExpense;

        private set
        {
            if (totalExpense != value)
            {
                totalExpense = value;
                OnPropertyChanged(nameof(TotalExpense));
            }
        }
    }

    public decimal TotalIncome
    {
        get => totalIncome;

        private set
        {
            if (totalIncome != value)
            {
                totalIncome = value;
                OnPropertyChanged(nameof(TotalIncome));
            }
        }
    }

    public bool HasData => CategorySummary.Any();

    public RelayCommand<CategoryOverviewViewModel> ShowCategoryPaymentsCommand => new(async vm => await ShowCategoryPaymentsAsync(vm));

    protected override async Task LoadAsync()
    {
        try
        {
            var categorySummaryModel = await Mediator.Send(
                new GetCategorySummary.Query(startDate: DateOnly.FromDateTime(StartDate), endDate: DateOnly.FromDateTime(EndDate)));

            CategorySummary = new(
                categorySummaryModel.CategoryOverviewItems.Select(
                    x => new CategoryOverviewViewModel
                    {
                        CategoryId = x.CategoryId,
                        Value = x.Value,
                        Average = x.Average,
                        Label = x.Label,
                        Percentage = x.Percentage
                    }));
        }
        catch (Exception ex)
        {
            Log.Warning(exception: ex, messageTemplate: "Error during loading");
            await dialogService.ShowMessageAsync(title: "Error", message: ex.ToString());
        }
    }

    private async Task ShowCategoryPaymentsAsync(CategoryOverviewViewModel categoryOverviewModel)
    {
        await Shell.Current.GoToModalAsync(Routes.PaymentForCategoryListRoute);
        Messenger.Send(new PaymentsForCategoryMessage(categoryId: categoryOverviewModel.CategoryId, startDate: StartDate, endDate: EndDate));
    }
}
