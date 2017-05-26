using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Representation of the MainView
    /// </summary>
    public class MainViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService navigationService;

        public MainViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public async Task ShowMenuAndFirstDetail()
        {
            await navigationService.Navigate<MenuViewModel>();
            await navigationService.Navigate<AccountListViewModel>();
        }
    }
}