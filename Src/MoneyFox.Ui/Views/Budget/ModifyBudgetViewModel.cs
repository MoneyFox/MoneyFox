namespace MoneyFox.Ui.Views.Budget;

using System.Collections;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using MoneyFox.Core.Common.Messages;
using MoneyFox.Core.Interfaces;
using MoneyFox.Ui.Views.Categories;
using ViewModels;

internal abstract class ModifyBudgetViewModel : BaseViewModel, IRecipient<CategorySelectedMessage>
{
    private readonly INavigationService navigationService;
    private BudgetViewModel selectedBudget = new();

    protected ModifyBudgetViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
        WeakReferenceMessenger.Default.Register(this);
    }

    public BudgetViewModel SelectedBudget
    {
        get => selectedBudget;
        private set => SetProperty(field: ref selectedBudget, newValue: value);
    }

    public ICollection TimeRangeCollection
        => new List<BudgetTimeRange>
        {
            BudgetTimeRange.YearToDate,
            BudgetTimeRange.Last1Year,
            BudgetTimeRange.Last2Years,
            BudgetTimeRange.Last3Years,
            BudgetTimeRange.Last5Years
        };

    // Todo: use ReadOnly Collection?
    public ObservableCollection<BudgetCategoryViewModel> SelectedCategories { get; set; } = new();

    public AsyncRelayCommand OpenCategorySelectionCommand => new(OpenCategorySelection);

    public RelayCommand<BudgetCategoryViewModel> RemoveCategoryCommand => new(RemoveCategory);

    public AsyncRelayCommand SaveBudgetCommand => new(SaveBudgetAsync);

    public void Receive(CategorySelectedMessage message)
    {
        var categorySelectedDataSet = message.Value;
        if (SelectedCategories.Any(c => c.CategoryId == message.Value.CategoryId) is false)
        {
            SelectedCategories.Add(new(categoryId: categorySelectedDataSet.CategoryId, name: categorySelectedDataSet.Name));
        }
    }

    private async Task OpenCategorySelection()
    {
        await navigationService.OpenModalAsync<SelectCategoryPage>();
    }

    private void RemoveCategory(BudgetCategoryViewModel? budgetCategory)
    {
        if (budgetCategory == null)
        {
            return;
        }

        SelectedCategories.Remove(budgetCategory);
    }

    protected abstract Task SaveBudgetAsync();
}
