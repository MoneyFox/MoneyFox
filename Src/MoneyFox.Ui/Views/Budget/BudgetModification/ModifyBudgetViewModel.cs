namespace MoneyFox.Ui.Views.Budget.BudgetModification;

using System.Collections.ObjectModel;
using Categories.CategorySelection;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Queries;
using Domain.Aggregates.BudgetAggregate;
using MediatR;

internal abstract class ModifyBudgetViewModel : BasePageViewModel, IQueryAttributable
{
    private readonly IDialogService dialogService;
    private readonly INavigationService navigationService;
    private readonly ISender sender;
    private string name = null!;
    private int numberOfMonths = 1;
    private decimal spendingLimit;

    protected ModifyBudgetViewModel(INavigationService navigationService, ISender sender, IDialogService dialogService)
    {
        this.navigationService = navigationService;
        this.sender = sender;
        this.dialogService = dialogService;
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

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(key: SelectCategoryViewModel.SELECTED_CATEGORY_ID_PARAM, value: out var selectedCategoryIdParam))
        {
            var selectedCategoryId = Convert.ToInt32(selectedCategoryIdParam);
            var category = sender.Send(new GetCategoryByIdQuery(selectedCategoryId)).GetAwaiter().GetResult();
            if (SelectedCategories.Any(c => c.CategoryId == selectedCategoryId) is false)
            {
                SelectedCategories.Add(new(categoryId: selectedCategoryId, name: category.Name));
            }
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

    private async Task SaveBudgetAsync()
    {
        try
        {
            await dialogService.ShowLoadingDialogAsync();
            await SaveAsync();
        }
        finally
        {
            await dialogService.HideLoadingDialogAsync();
        }
    }

    protected abstract Task SaveAsync();
}
