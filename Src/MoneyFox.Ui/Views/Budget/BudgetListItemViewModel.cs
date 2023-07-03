namespace MoneyFox.Ui.Views.Budget;

using CommunityToolkit.Mvvm.ComponentModel;

public sealed class BudgetListItemViewModel : ObservableObject
{
    private readonly decimal monthlyBudget;
    private readonly decimal monthlySpending;
    private decimal currentSpending;
    private string name = null!;
    private decimal spendingLimit;

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

    public required decimal MonthlySpending
    {
        get => monthlySpending;
        init => SetProperty(field: ref monthlySpending, newValue: value);
    }
}
