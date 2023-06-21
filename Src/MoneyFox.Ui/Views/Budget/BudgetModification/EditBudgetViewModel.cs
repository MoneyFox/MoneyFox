namespace MoneyFox.Ui.Views.Budget.BudgetModification;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Features.BudgetDeletion;
using Core.Features.BudgetUpdate;
using Core.Queries.BudgetEntryLoading;
using Domain.Aggregates.BudgetAggregate;
using MediatR;
using Navigation;
using Resources.Strings;

internal sealed class EditBudgetViewModel : ModifyBudgetViewModel
{
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ISender sender;

    private bool isFirstLoad = true;

    public EditBudgetViewModel(ISender sender, INavigationService navigationService, IDialogService dialogService) : base(
        navigationService: navigationService,
        sender: sender)
    {
        this.sender = sender;
        this.navigationService = navigationService;
        this.dialogService = dialogService;
    }

    public BudgetId Id { get; private set; }

    public AsyncRelayCommand<int> InitializeCommand => new(InitializeAsync);

    public AsyncRelayCommand DeleteBudgetCommand => new(DeleteBudgetAsync);

    public override async Task OnNavigatedAsync(object? parameter)
    {

        var selectedBudgetId = Convert.ToInt32(parameter);
        await InitializeAsync(selectedBudgetId);
    }

    private async Task InitializeAsync(int budgetId)
    {
        if (isFirstLoad is false)
        {
            return;
        }

        var query = new LoadBudgetEntry.Query(budgetId: budgetId);
        var budgetData = await sender.Send(query);
        Id = budgetData.Id;
        Name = budgetData.Name;
        SpendingLimit = budgetData.SpendingLimit;
        NumberOfMonths = budgetData.NumberOfMonths;
        SelectedCategories.Clear();
        SelectedCategories.AddRange(budgetData.Categories.Select(bc => new BudgetCategoryViewModel(categoryId: bc.Id, name: bc.Name)));
        isFirstLoad = false;
    }

    private async Task DeleteBudgetAsync()
    {
        if (await dialogService.ShowConfirmMessageAsync(title: Translations.DeleteTitle, message: Translations.DeleteBudgetConfirmationMessage))
        {
            var command = new DeleteBudget.Command(budgetId: Id);
            await sender.Send(command);
            await navigationService.NavigateBackAsync();
            _ = Messenger.Send(new BudgetsChangedMessage());
        }
    }

    protected override async Task SaveBudgetAsync()
    {
        var command = new UpdateBudget.Command(
            budgetId: Id,
            name: Name,
            spendingLimit: SpendingLimit,
            numberOfMonths: NumberOfMonths,
            categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

        await sender.Send(command);
        await navigationService.NavigateBackAsync();
        _ = Messenger.Send(new BudgetsChangedMessage());
    }
}
