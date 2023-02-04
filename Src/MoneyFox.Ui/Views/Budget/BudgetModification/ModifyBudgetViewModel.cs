namespace MoneyFox.Ui.Views.Budget.BudgetModification;

using System.Collections.ObjectModel;
using Categories.CategorySelection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Interfaces;
using Domain.Aggregates.BudgetAggregate;
using Messages;

internal abstract class ModifyBudgetViewModel : BasePageViewModel, IRecipient<CategorySelectedMessage>
{
    private readonly INavigationService navigationService;
    private string name = null!;
    private decimal spendingLimit;
    private int numberOfMonths;

    protected ModifyBudgetViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    public string Name
    {
        get => name;

        set
        {
            SetProperty(field: ref name, newValue: value);
            OnPropertyChanged(nameof(IsValid));
        }
    }

    public decimal SpendingLimit
    {
        get => spendingLimit;

        set
        {
            SetProperty(field: ref spendingLimit, newValue: value);
            OnPropertyChanged(nameof(IsValid));
        }
    }

    public int NumberOfMonths
    {
        get => numberOfMonths;

        set
        {
            SetProperty(field: ref numberOfMonths, newValue: value);
            OnPropertyChanged(nameof(IsValid));
        }
    }

    public bool IsValid => string.IsNullOrEmpty(Name) is false && SpendingLimit > 0 && NumberOfMonths > 0;

    public static List<BudgetTimeRange> TimeRangeCollection
        => new()
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

    public AsyncRelayCommand SaveBudgetCommand => new(execute: SaveBudgetAsync, canExecute: () => IsValid);

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
