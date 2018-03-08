using System.Threading.Tasks;
using MoneyFox.Business.ViewModels.Statistic;
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

        public MvxAsyncCommand ShowInitialViewModelsCommand => new MvxAsyncCommand(ShowInitialViewModels);

        private async Task ShowInitialViewModels()
        {
            await navigationService.Navigate<AccountListViewModel>();
            await navigationService.Navigate<StatisticSelectorViewModel>();
            await navigationService.Navigate<SettingsViewModel>();
        }
    }
}