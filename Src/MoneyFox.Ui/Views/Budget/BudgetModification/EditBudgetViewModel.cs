namespace MoneyFox.Ui.Views.Budget.BudgetModification;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Extensions;
using Core.Common.Interfaces;
using Core.Features.BudgetDeletion;
using Core.Features.BudgetUpdate;
using Core.Queries.BudgetEntryLoading;
using Domain.Aggregates.BudgetAggregate;
using MediatR;
using Resources.Strings;

internal sealed class EditBudgetViewModel : ModifyBudgetViewModel
{
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ISender sender;

    public EditBudgetViewModel(ISender sender, INavigationService navigationService, IDialogService dialogService) : base(
        navigationService: navigationService,
        sender: sender,
        dialogService: dialogService)
    {
        this.sender = sender;
        this.navigationService = navigationService;
        this.dialogService = dialogService;
    }

    public BudgetId Id { get; private set; }

    public AsyncRelayCommand DeleteBudgetCommand => new(DeleteBudgetAsync);

    public override async Task OnNavigatedAsync(object? parameter)
    {
        var budgetId = Convert.ToInt32(parameter);
        var budgetData = await sender.Send(new LoadBudgetEntry.Query(budgetId: budgetId));
        Id = budgetData.Id;
        Name = budgetData.Name;
        SpendingLimit = budgetData.SpendingLimit;
        NumberOfMonths = budgetData.NumberOfMonths;
        SelectedCategories.Clear();
        SelectedCategories.AddRange(budgetData.Categories.Select(bc => new BudgetCategoryViewModel(categoryId: bc.Id, name: bc.Name)));
    }

    private async Task DeleteBudgetAsync()
    {
        if (await dialogService.ShowConfirmMessageAsync(title: Translations.DeleteTitle, message: Translations.DeleteBudgetConfirmationMessage))
        {
            var command = new DeleteBudget.Command(budgetId: Id);
            await sender.Send(command);
            await navigationService.GoBack();
        }
    }

    protected override async Task SaveAsync()
    {
        var command = new UpdateBudget.Command(
            budgetId: Id,
            name: Name,
            spendingLimit: SpendingLimit,
            numberOfMonths: NumberOfMonths,
            categories: SelectedCategories.Select(sc => sc.CategoryId).ToList());

        await sender.Send(command);
        await navigationService.GoBack();
    }
}
