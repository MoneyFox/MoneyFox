namespace MoneyFox.Ui.Views.Budget;

using CommunityToolkit.Mvvm.ComponentModel;

public sealed class BudgetListItemViewModel : ObservableObject
{
    private decimal currentSpending;
    private string name = null!;

    private decimal spendingLimit;
    public int Id { get; set; }

    public string Name
    {
        get => name;
        set
        {
            if (name == value)
            {
                return;
            }

            name = value;
            OnPropertyChanged();
        }
    }

    public double SpendingPercentage => (double)CurrentSpending / (double)SpendingLimit;

    public decimal CurrentSpending
    {
        get => currentSpending;
        set
        {
            if (currentSpending == value)
            {
                return;
            }

            currentSpending = value;
            OnPropertyChanged();
        }
    }

    public decimal SpendingLimit
    {
        get => spendingLimit;
        set
        {
            if (spendingLimit == value)
            {
                return;
            }
            spendingLimit = value;
            OnPropertyChanged();
        }
    }
}
