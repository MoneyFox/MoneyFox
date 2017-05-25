using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Represents the side menu
    /// </summary>
    public class MenuViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService navigationService;

        public MenuViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public MvxAsyncCommand ShowAccountListCommand
            => new MvxAsyncCommand(async () => await navigationService.Navigate<AccountListViewModel>());
    }
}