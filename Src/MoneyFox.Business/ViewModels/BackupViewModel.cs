using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Microsoft.Graph;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using Plugin.Connectivity.Abstractions;

namespace MoneyFox.Business.ViewModels
{
    public interface IBackupViewModel : IBaseViewModel
    {
        MvxAsyncCommand LoginCommand { get; }
        MvxAsyncCommand LogoutCommand { get; }
        MvxAsyncCommand BackupCommand { get; }
        MvxAsyncCommand RestoreCommand { get; }

        DateTime BackupLastModified { get; }
        bool IsLoadingBackupAvailability { get; }
        bool IsLoggedIn { get; }
        bool BackupAvailable { get; }
    }

    /// <summary>
    ///     Representation of the backup view.
    /// </summary>
    public class BackupViewModel : BaseNavigationViewModel, IBackupViewModel
    {
        private readonly IBackupManager backupManager;
        private readonly IConnectivity connectivity;
        private readonly IDialogService dialogService;
        private readonly ISettingsManager settingsManager;
        private bool backupAvailable;

        private DateTime backupLastModified;
        private bool isLoadingBackupAvailability;

        public BackupViewModel(IBackupManager backupManager,
                               IDialogService dialogService,
                               IConnectivity connectivity,
                               ISettingsManager settingsManager,
                               IMvxLogProvider logProvider,
                               IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.backupManager = backupManager;
            this.dialogService = dialogService;
            this.connectivity = connectivity;
            this.settingsManager = settingsManager;
        }

        #region Properties

        /// <summary>
        ///     Makes the first login and sets the setting for the future navigations to this page.
        /// </summary>
        public MvxAsyncCommand LoginCommand => new MvxAsyncCommand(Login);

        /// <summary>
        ///     Logs the user out from the backup service.
        /// </summary>
        public MvxAsyncCommand LogoutCommand => new MvxAsyncCommand(Logout);

        /// <summary>
        ///     Will create a backup of the database and upload it to onedrive
        /// </summary>
        public MvxAsyncCommand BackupCommand => new MvxAsyncCommand(CreateBackup);

        /// <summary>
        ///     Will download the database backup from onedrive and overwrite the
        ///     local database with the downloaded.
        ///     All datamodels are then reloaded.
        /// </summary>
        public MvxAsyncCommand RestoreCommand => new MvxAsyncCommand(RestoreBackup);

        /// <summary>
        ///     The Date when the backup was modified the last time.
        /// </summary>
        public DateTime BackupLastModified
        {
            get => backupLastModified;
            private set
            {
                if (backupLastModified == value) return;
                backupLastModified = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Indicator that the app is checking if backups available.
        /// </summary>
        public bool IsLoadingBackupAvailability
        {
            get => isLoadingBackupAvailability;
            private set
            {
                if (isLoadingBackupAvailability == value) return;
                isLoadingBackupAvailability = value;
                RaisePropertyChanged();
            }
        }
        
        /// <summary>
        ///     Indicator that the user logged in to the backup service.
        /// </summary>
        public bool IsLoggedIn => settingsManager.IsLoggedInToBackupService;

        /// <summary>
        ///     Indicates if a backup is available for restore.
        /// </summary>
        public bool BackupAvailable
        {
            get => backupAvailable;
            private set
            {
                if (backupAvailable == value) return;
                backupAvailable = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public override async Task Initialize()
        {
            await Loaded();
        }

        private async Task Loaded()
        {
            if (!IsLoggedIn)
            {
                return;
            }

            if (!connectivity.IsConnected)
            {
                await dialogService.ShowMessage(Strings.NoNetworkTitle, Strings.NoNetworkMessage);
            }

            IsLoadingBackupAvailability = true;
            try
            {
                BackupAvailable = await backupManager.IsBackupExisting();
                BackupLastModified = await backupManager.GetBackupDate();
            }
            catch (BackupAuthenticationFailedException ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string> { { "Info", "Issue during Login process." } });
                await backupManager.Logout();
                await dialogService.ShowMessage(Strings.AuthenticationFailedTitle,
                                                Strings.ErrorMessageAuthenticationFailed);
            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                if (ex.Error.Code == "4f37.717b")
                {
                    Crashes.TrackError(ex, new Dictionary<string, string> { { "Info", "Graph Login Exception" } });
                    await backupManager.Logout();
                    await dialogService.ShowMessage(Strings.AuthenticationFailedTitle,
                                                    Strings.ErrorMessageAuthenticationFailed);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>{{ "Info", "Unknown Issue"}});
                await dialogService.ShowMessage(Strings.GeneralErrorTitle,
                                                ex.ToString());
            }

            IsLoadingBackupAvailability = false;
        }

        private async Task Login()
        {
            if (!connectivity.IsConnected)
            {
                await dialogService.ShowMessage(Strings.NoNetworkTitle, Strings.NoNetworkMessage);
            }

            try
            {
                await backupManager.Login();
                settingsManager.IsLoggedInToBackupService = true;
                // ReSharper disable once ExplicitCallerInfoArgument
                await RaisePropertyChanged(nameof(IsLoggedIn));
            }
            catch (BackupAuthenticationFailedException)
            {
                await dialogService.ShowMessage(Strings.AuthenticationFailedTitle,
                                                Strings.ErrorMessageAuthenticationFailed);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await dialogService.ShowMessage(Strings.LoginFailedTitle,
                                                Strings.LoginFailedMessage);
            }

            await Loaded();
        }

        private async Task Logout()
        {
            await backupManager.Logout();
            settingsManager.IsLoggedInToBackupService = false;
            // ReSharper disable once ExplicitCallerInfoArgument
            await RaisePropertyChanged(nameof(IsLoggedIn));
        }

        private async Task CreateBackup()
        {
            if (!await ShowOverwriteBackupInfo())
            {
                return;
            }

            dialogService.ShowLoadingDialog();
            try
            {
                await backupManager.CreateNewBackup();
                BackupLastModified = DateTime.Now;
            }
            catch (BackupAuthenticationFailedException)
            {
                await backupManager.Logout();
                await dialogService.ShowMessage(Strings.AuthenticationFailedTitle,
                                                Strings.ErrorMessageAuthenticationFailed);
            } 
            catch (ServiceException ex)
            {
                await backupManager.Logout();
                Crashes.TrackError(ex);
            } 
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await dialogService.ShowMessage(Strings.BackupFailedTitle,
                                                Strings.ErrorMessageBackupFailed);
            } 

            dialogService.HideLoadingDialog();
            await ShowCompletionNote();
        }

        private async Task RestoreBackup()
        {
            if (!await ShowOverwriteDataInfo())
            {
                return;
            }

            dialogService.ShowLoadingDialog();
            try
            {
                await backupManager.RestoreBackup();
            }
            catch (BackupAuthenticationFailedException ex)
            {
                await backupManager.Logout();
                Crashes.TrackError(ex);
                await dialogService.ShowMessage(Strings.AuthenticationFailedTitle,
                                                Strings.ErrorMessageAuthenticationFailed);
            }
            catch (ServiceException ex)
            {
                await backupManager.Logout();
                Crashes.TrackError(ex);
                await dialogService.ShowMessage(Strings.BackupRestoreFailedTitle,
                                                Strings.ErrorMessageRestore);
            } 
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await dialogService.ShowMessage(Strings.BackupFailedTitle,
                                                Strings.ErrorMessageBackupFailed);
            }

            dialogService.HideLoadingDialog();
            await ShowCompletionNote();
        }

        private async Task<bool> ShowOverwriteBackupInfo()
            => await dialogService.ShowConfirmMessage(Strings.OverwriteTitle, Strings.OverwriteBackupMessage);

        private async Task<bool> ShowOverwriteDataInfo()
            => await dialogService.ShowConfirmMessage(Strings.OverwriteTitle, Strings.OverwriteDataMessage);

        private async Task ShowCompletionNote()
            => await dialogService.ShowMessage(Strings.SuccessTitle, Strings.TaskSuccessfulMessage);
    }
}