namespace MoneyFox.Ui.Views.Budget;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Domain.Aggregates.BudgetAggregate;

public class BudgetViewModel : ObservableObject
{
    private string name = null!;
    private decimal spendingLimit;
    private BudgetTimeRange timeRange;

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

    public ObservableCollection<BudgetCategoryViewModel> SelectedCategories { get; set; } = new();

    public bool IsValid => string.IsNullOrEmpty(Name) is false && SpendingLimit > 0;
}
