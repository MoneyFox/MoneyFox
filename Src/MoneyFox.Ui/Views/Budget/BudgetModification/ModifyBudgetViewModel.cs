namespace MoneyFox.Ui.Views.Budget.BudgetModification;

using System.Collections.ObjectModel;
using Categories.CategorySelection;
using CommunityToolkit.Mvvm.Input;
using Core.Queries;
using Domain.Aggregates.BudgetAggregate;
using MediatR;
using Navigation;

internal abstract class ModifyBudgetViewModel : BasePageViewModel
{
    private readonly INavigationService navigationService;
    private readonly ISender sender;
    private string name = null!;
    private int numberOfMonths = 1;
    private decimal spendingLimit;

    protected ModifyBudgetViewModel(INavigationService navigationService, ISender sender)
    {
        this.navigationService = navigationService;
        this.sender = sender;
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

    public override async Task OnNavigatedBackAsync(object? parameter)
    {
        var selectedCategoryId = Convert.ToInt32(parameter);
        var category = await sender.Send(new GetCategoryByIdQuery(selectedCategoryId));
        if (SelectedCategories.Any(c => c.CategoryId == selectedCategoryId) is false)
        {
            SelectedCategories.Add(new(categoryId: selectedCategoryId, name: category.Name));
        }
    }

    private async Task OpenCategorySelection()
    {
        await navigationService.NavigateToViewModelAsync<SelectCategoryViewModel>(modalNavigation:true);
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
