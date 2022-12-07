namespace MoneyFox.Ui.Views.Budget;

using System.Collections;
using System.Collections.ObjectModel;
using Categories;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using Core.Common.Messages;
using Core.Interfaces;
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

        private set
        {
            SetProperty(field: ref selectedBudget, newValue: value);
            SaveBudgetCommand.NotifyCanExecuteChanged();
        }
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

    public AsyncRelayCommand SaveBudgetCommand => new(SaveBudgetAsync, canExecute: () => string.IsNullOrEmpty(SelectedBudget.Name) is false);

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

