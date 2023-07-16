namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using System.Collections.ObjectModel;
using Common.Extensions;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Queries.Statistics.GetCategorySummary;
using MediatR;
using MoneyFox.Core.Common.Settings;
using MoneyFox.Ui.Infrastructure.Adapters;
using Serilog;

internal sealed class StatisticCategorySummaryViewModel : StatisticViewModel
{
    private readonly IDialogService dialogService;
    private readonly ISettingsFacade settingsFacade;

    private ObservableCollection<CategoryOverviewViewModel> categorySummary = new();
    private Observable<string> totalExpenseString = new();
    private Observable<string> totalIncomeString = new();

    public StatisticCategorySummaryViewModel(IMediator mediator, IDialogService dialogService) : base(mediator)
    {
        this.dialogService = dialogService;
        CategorySummary = new();
        settingsFacade = new SettingsFacade(new SettingsAdapter());
    }

    public ObservableCollection<CategoryOverviewViewModel> CategorySummary
    {
        get => categorySummary;

        private set
        {
            categorySummary = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasData));
        }
    }

    public Observable<string> TotalExpenseString
    {
        get => totalExpenseString;

        private set
        {
            if(TotalExpenseString != value)
            {
                totalExpenseString = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasData));
            }
        }
    }

    public Observable<string> TotalIncomeString
    {
        get => totalIncomeString;

        private set
        {
            if (totalIncomeString != value)
            {
                totalIncomeString = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasData));
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

            totalExpenseString.Content = $"Total Spending: {Math.Abs(categorySummary.Where(x => x.Value < 0).Sum(x => x.Value))} {settingsFacade.DefaultCurrency}";

            totalIncomeString.Content = $"Total Income: {categorySummary.Where(x => x.Value > 0).Sum(x => x.Value)} {settingsFacade.DefaultCurrency}";
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
