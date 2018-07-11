using System.Threading.Tasks;
using MoneyFox.Business.ViewModels.Statistic;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.Connectivity.Abstractions;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Representation of the MainView
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService navigationService;

        public MainViewModel(ISettingsManager settingsManager,
                                 IPasswordStorage passwordStorage,
                                 ITileManager tileManager,
                                 IBackgroundTaskManager backgroundTaskManager,
                                 IDialogService dialogService,
                                 IMvxNavigationService navigationService,
                                 IBackupManager backupManager,
                                 IConnectivity connectivity)
        {
            this.navigationService = navigationService;

            SettingsViewModel = new SettingsViewModel(settingsManager, passwordStorage, tileManager,backgroundTaskManager,
                dialogService, navigationService, backupManager, connectivity);
        }

        public SettingsViewModel SettingsViewModel { get; }

        public MvxAsyncCommand ShowInitialViewModelsCommand => new MvxAsyncCommand(ShowInitialViewModels);

        public MvxAsyncCommand ShowAccountListCommand
            => new MvxAsyncCommand(async () => await navigationService.Navigate<AccountListViewModel>());

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
            await navigationService.Navigate<AccountListViewModel>();
            await navigationService.Navigate<StatisticSelectorViewModel>();
            await navigationService.Navigate<SettingsViewModel>();
        }
    }
}