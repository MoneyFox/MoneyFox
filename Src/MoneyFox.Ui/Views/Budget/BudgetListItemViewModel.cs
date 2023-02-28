namespace MoneyFox.Ui.Views.Budget;

using CommunityToolkit.Mvvm.ComponentModel;

public sealed class BudgetListItemViewModel : ObservableViewModelBase
{
    private decimal currentSpending;
    private string name = null!;

    private decimal spendingLimit;
    public int Id { get; set; }

    public string Name
    {
        get => name;
        set => SetProperty( ref name,   value);
    }

    public double SpendingPercentage => (double)CurrentSpending / (double)SpendingLimit;

    public decimal CurrentSpending
    {
        get => currentSpending;
        set => SetProperty( ref currentSpending,   value);
    }

    public decimal SpendingLimit
    {
        get => spendingLimit;
        set => SetProperty( ref spendingLimit,   value);
    }
}
