namespace MoneyFox.Ui.Views.Budget.BudgetModification;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Features.BudgetDeletion;
using Core.Features.BudgetUpdate;
using Core.Interfaces;
using Core.Queries.BudgetEntryLoading;
using Domain.Aggregates.BudgetAggregate;
using MediatR;
using Resources.Strings;

internal sealed class EditBudgetViewModel : ModifyBudgetViewModel
{
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ISender sender;

    private bool isFirstLoad = true;

    public EditBudgetViewModel(ISender sender, INavigationService navigationService, IDialogService dialogService) : base(navigationService: navigationService)
    {
        this.sender = sender;
        this.navigationService = navigationService;
        this.dialogService = dialogService;
    }

    public BudgetId Id { get; private set; }

    public AsyncRelayCommand<int> InitializeCommand => new(InitializeAsync);

    public AsyncRelayCommand DeleteBudgetCommand => new(DeleteBudgetAsync);

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
            _ = await sender.Send(command);
            await navigationService.GoBackFromModalAsync();
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

        _ = await sender.Send(command);
        await navigationService.GoBackFromModalAsync();
        _ = Messenger.Send(new BudgetsChangedMessage());
    }
}
