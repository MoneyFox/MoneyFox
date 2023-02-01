namespace MoneyFox.Ui.Views.Budget;

using System.Collections;
using System.Collections.ObjectModel;
using Categories;
using Categories.CategorySelection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Messages;
using Core.Interfaces;
using Domain.Aggregates.BudgetAggregate;

internal abstract class ModifyBudgetViewModel : BaseViewModel, IRecipient<CategorySelectedMessage>
{
    private readonly INavigationService navigationService;
    private string name = null!;
    private decimal spendingLimit;
    private BudgetTimeRange timeRange;

    protected ModifyBudgetViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
        WeakReferenceMessenger.Default.Register(this);
    }

    public string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }

    public BudgetTimeRange TimeRange
    {
        get => timeRange;
        set => SetProperty(field: ref timeRange, newValue: value);
    }

    public decimal SpendingLimit
    {
        get => spendingLimit;
        set => SetProperty(field: ref spendingLimit, newValue: value);
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

    public ObservableCollection<BudgetCategoryViewModel> SelectedCategories { get; set; } = new();

    public AsyncRelayCommand OpenCategorySelectionCommand => new(OpenCategorySelection);

    public RelayCommand<BudgetCategoryViewModel> RemoveCategoryCommand => new(RemoveCategory);

    public AsyncRelayCommand SaveBudgetCommand => new(execute: SaveBudgetAsync, canExecute: () => string.IsNullOrEmpty(Name) is false && SpendingLimit > 0);

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

        _ = SelectedCategories.Remove(budgetCategory);
    }

    protected abstract Task SaveBudgetAsync();
}
