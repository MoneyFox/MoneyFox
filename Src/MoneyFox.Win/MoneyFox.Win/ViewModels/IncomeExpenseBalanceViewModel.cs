#nullable enable
namespace MoneyFox.Win.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

public class IncomeExpenseBalanceViewModel : ObservableObject
{
    private decimal totalEarned;
    private decimal totalSpent;

    public decimal TotalEarned
    {
        get => totalEarned;
        set
        {
            if(totalEarned == value)
            {
                return;
            }

            totalEarned = value;
            OnPropertyChanged();
        }
    }

    public decimal TotalSpent
    {
        get => totalSpent;
        set
        {
            if(totalSpent == value)
            {
                return;
            }

            totalSpent = value;
            OnPropertyChanged();
        }
    }
}