using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyFox.Win.Services;
using MoneyFox.Win.ViewModels.Interfaces;

namespace MoneyFox.Win.ViewModels.Accounts
{
    public class AccountListViewActionViewModel : ObservableObject, IAccountListViewActionViewModel
    {
        private readonly NavigationService navigationService;

        public AccountListViewActionViewModel(NavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        /// <inheritdoc />
        public RelayCommand GoToAddAccountCommand =>
            new RelayCommand(() => navigationService.Navigate<AddAccountViewModel>());
    }
}