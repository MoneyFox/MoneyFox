using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Interfaces;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Accounts
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