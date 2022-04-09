namespace MoneyFox.Win.ViewModels.DesignTime;

using CommunityToolkit.Mvvm.Input;
using Interfaces;

public class DesignTimeAccountListViewActionViewModel : IAccountListViewActionViewModel
{
    public RelayCommand GoToAddIncomeCommand { get; } = null!;

    public RelayCommand GoToAddExpenseCommand { get; } = null!;

    public RelayCommand GoToAddTransferCommand { get; } = null!;

    public bool IsAddIncomeAvailable { get; }

    public bool IsAddExpenseAvailable { get; }

    public bool IsTransferAvailable { get; }

    public RelayCommand GoToAddAccountCommand { get; } = null!;
}
