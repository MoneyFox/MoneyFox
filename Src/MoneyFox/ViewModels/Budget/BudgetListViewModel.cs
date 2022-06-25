namespace MoneyFox.ViewModels.Budget;

using CommunityToolkit.Mvvm.ComponentModel;

public sealed class BudgetListViewModel : ObservableObject
{
    private string name = null!;

    private decimal currentSpending;

    private decimal spendingLimit;
    public int Id { get; set; }

    public string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }

    public decimal CurrentSpending
    {
        get => currentSpending;
        set => SetProperty(field: ref currentSpending, newValue: value);
    }

    public decimal SpendingLimit
    {
        get => spendingLimit;
        set => SetProperty(field: ref spendingLimit, newValue: value);
    }
}
