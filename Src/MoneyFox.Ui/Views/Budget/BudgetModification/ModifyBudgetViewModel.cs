namespace MoneyFox.Ui.Views.Budget.BudgetModification;

using System.Collections.ObjectModel;
using Categories.CategorySelection;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Queries;
using Domain.Aggregates.BudgetAggregate;
using MediatR;

internal abstract class ModifyBudgetViewModel(INavigationService navigationService, ISender sender, IDialogService service) : NavigableViewModel
{
    private string name = null!;
    private int numberOfMonths = 1;
    private decimal spendingLimit;

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

    private Task OpenCategorySelection()
    {
        return navigationService.GoTo<SelectCategoryViewModel>();
    }

    private void RemoveCategory(BudgetCategoryViewModel? budgetCategory)
    {
        if (budgetCategory == null)
        {
            return;
        }

        SelectedCategories.Remove(budgetCategory);
    }

    private async Task SaveBudgetAsync()
    {
        try
        {
            await service.ShowLoadingDialogAsync();
            await SaveAsync();
        }
        finally
        {
            await service.HideLoadingDialogAsync();
        }
    }

    protected abstract Task SaveAsync();
}
