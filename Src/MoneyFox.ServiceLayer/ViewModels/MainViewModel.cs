using System.Threading.Tasks;
using MoneyFox.ServiceLayer.ViewModels.Statistic;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Representation of the MainView
    /// </summary>
    public class MainViewModel : BaseNavigationViewModel
    {
        private readonly IMvxNavigationService navigationService;

        public MainViewModel(IMvxLogProvider logProvider,
                        IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.navigationService = navigationService;
        }

        public MvxAsyncCommand ShowInitialViewModelsCommand => new MvxAsyncCommand(ShowInitialViewModels);

        //public MvxAsyncCommand ShowAccountListCommand
        //    => new MvxAsyncCommand(async () => await navigationService.Navigate<AccountListViewModel>());

        public MvxAsyncCommand ShowStatisticSelectorCommand
            => new MvxAsyncCommand(async () => await navigationService.Navigate<StatisticSelectorViewModel>());

        public MvxAsyncCommand ShowCategoryListCommand
            => new MvxAsyncCommand(async () => await navigationService.Navigate<CategoryListViewModel>());

        public MvxAsyncCommand ShowBackupViewCommand
            => new MvxAsyncCommand(async () => await navigationService.Navigate<BackupViewModel>());

        public MvxAsyncCommand ShowSettingsCommand
            => new MvxAsyncCommand(async () => await navigationService.Navigate<SettingsViewModel>());

        public MvxAsyncCommand ShowAboutCommand
            => new MvxAsyncCommand(async () => await navigationService.Navigate<AboutViewModel>());

        private async Task ShowInitialViewModels()
        {
            //await navigationService.Navigate<AccountListViewModel>();
            await navigationService.Navigate<StatisticSelectorViewModel>();
            await navigationService.Navigate<SettingsViewModel>();
        }
    }
}