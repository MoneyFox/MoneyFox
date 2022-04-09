namespace MoneyFox.Win.ViewModels.Interfaces;

using CommunityToolkit.Mvvm.Input;

public interface IAccountListViewActionViewModel
{
    RelayCommand GoToAddAccountCommand { get; }
}
