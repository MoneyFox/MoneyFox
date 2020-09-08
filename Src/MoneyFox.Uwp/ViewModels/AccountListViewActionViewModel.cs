using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Interfaces;

namespace MoneyFox.Uwp.ViewModels
{
    public class AccountListViewActionViewModel : ViewModelBase, IAccountListViewActionViewModel
    {
        private readonly NavigationService navigationService;

        public AccountListViewActionViewModel(NavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        /// <inheritdoc/>
        public RelayCommand GoToAddAccountCommand => new RelayCommand(async () => await navigationService.CreateNewViewAsync(ViewModelLocator.AddAccount));
    }
}
