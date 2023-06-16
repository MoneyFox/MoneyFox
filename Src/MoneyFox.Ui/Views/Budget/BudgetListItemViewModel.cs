namespace MoneyFox.Ui.Views.Budget;

using CommunityToolkit.Mvvm.ComponentModel;

public sealed class BudgetListItemViewModel : ObservableObject
{
    private string name = null!;
    private decimal currentSpending;
    private decimal spendingLimit;
    private readonly decimal monthlyBudget;

    public int Id { get; init; }

    public required string Name
    {
        get => name;
        set => SetProperty(field: ref name, newValue: value);
    }

    public required decimal CurrentSpending
    {
        get => currentSpending;
        set => SetProperty(field: ref currentSpending, newValue: value);
    }

    public required decimal SpendingLimit
    {
        get => spendingLimit;
        set => SetProperty(field: ref spendingLimit, newValue: value);
    }

    public double SpendingPercentage => (double)CurrentSpending / (double)SpendingLimit;

    public required decimal MonthlyBudget
    {
        get => monthlyBudget;
        init => SetProperty(field: ref monthlyBudget, newValue: value);
    }
}
