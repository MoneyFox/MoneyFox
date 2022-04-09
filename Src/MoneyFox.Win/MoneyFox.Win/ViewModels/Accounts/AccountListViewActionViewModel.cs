namespace MoneyFox.Win.ViewModels.Accounts;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Interfaces;
using Services;

public class AccountListViewActionViewModel : ObservableObject, IAccountListViewActionViewModel
{
    private readonly INavigationService navigationService;

    public AccountListViewActionViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
    }

    public RelayCommand GoToAddAccountCommand => new(() => navigationService.Navigate<AddAccountViewModel>());
}
