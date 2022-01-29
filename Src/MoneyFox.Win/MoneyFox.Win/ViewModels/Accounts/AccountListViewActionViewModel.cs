using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyFox.Win.Services;
using MoneyFox.Win.ViewModels.Interfaces;

namespace MoneyFox.Win.ViewModels.Accounts
{
    public class AccountListViewActionViewModel : ObservableObject, IAccountListViewActionViewModel
    {
        private readonly INavigationService navigationService;

        public AccountListViewActionViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public RelayCommand GoToAddAccountCommand =>
            new RelayCommand(() => navigationService.Navigate<AddAccountViewModel>());
    }
}