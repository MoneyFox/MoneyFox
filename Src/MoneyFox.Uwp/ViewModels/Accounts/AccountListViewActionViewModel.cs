using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Interfaces;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Accounts
{
    public class AccountListViewActionViewModel : ViewModelBase, IAccountListViewActionViewModel
    {
        private readonly NavigationService navigationService;

        public AccountListViewActionViewModel(NavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        /// <inheritdoc />
        public RelayCommand GoToAddAccountCommand
            => new RelayCommand(() => navigationService.Navigate<AddAccountViewModel>());
    }
}