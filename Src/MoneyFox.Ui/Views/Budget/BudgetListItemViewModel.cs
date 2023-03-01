namespace MoneyFox.Ui.Views.Budget;

public sealed class BudgetListItemViewModel : ObservableViewModelBase
{
    private decimal currentSpending;
    private string name = null!;

    private decimal spendingLimit;
    public int Id { get; set; }

    public string Name
    {
        get => name;
        set => SetProperty(property: ref name, value: value);
    }

    public double SpendingPercentage => (double)CurrentSpending / (double)SpendingLimit;

    public decimal CurrentSpending
    {
        get => currentSpending;
        set => SetProperty(property: ref currentSpending, value: value);
    }

    public decimal SpendingLimit
    {
        get => spendingLimit;
        set => SetProperty(property: ref spendingLimit, value: value);
    }
}
