using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Email;
using MvvmCross.Plugin.WebBrowser;
using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyFox.Business.ViewModels
{
    public interface IGeneralViewModel : IBaseViewModel
    {

    }
    /// <summary>
    ///     ViewModel for the General view.
    /// </summary>
    public class GeneralViewModel: BaseViewModel, IGeneralViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public GeneralViewModel(ISettingsManager settingsManager,
                                 IPasswordStorage passwordStorage,
                                 ITileManager tileManager,
                                 IBackgroundTaskManager backgroundTaskManager,
                                 IDialogService dialogService,
                                 IMvxNavigationService navigationService,
                                 IBackupManager backupManager,
                                 IConnectivity connectivity, 
                                 IAppInformation appInformation,
                                IMvxComposeEmailTask composeEmailTask,
                                IMvxWebBrowserTask webBrowserTask,
                                IStoreOperations storeOperations)
        {
            _navigationService = navigationService;

            SettingsViewModel = new SettingsViewModel(settingsManager, passwordStorage, tileManager, backgroundTaskManager, dialogService, backupManager, connectivity);
            AboutViewModel = new AboutViewModel(appInformation, composeEmailTask, webBrowserTask, storeOperations);
        }

        public SettingsViewModel SettingsViewModel { get; }
        public AboutViewModel AboutViewModel { get; }

        public string AboutLabel => Strings.AboutLabel;
        public string SettingsLabel => Strings.SettingsLabel;
    }
}
