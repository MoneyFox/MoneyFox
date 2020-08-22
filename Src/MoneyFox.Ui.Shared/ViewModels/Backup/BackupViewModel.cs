using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Graph;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Exceptions;
using MoneyFox.Services;
using NLog;
using System;
using System.Threading.Tasks;

namespace MoneyFox.Ui.Shared.ViewModels.Backup
{
    /// <summary>
    /// Representation of the backup view.
    /// </summary>
    public class BackupViewModel : ViewModelBase, IBackupViewModel
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IBackupService backupService;
        private readonly IConnectivityAdapter connectivity;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;
        private readonly IToastService toastService;
        private bool backupAvailable;

        private DateTime backupLastModified;
        private bool isLoadingBackupAvailability;

        public BackupViewModel(IBackupService backupService,
                               IDialogService dialogService,
                               IConnectivityAdapter connectivity,
                               ISettingsFacade settingsFacade,
                               IToastService toastService)
        {
            this.backupService = backupService;
            this.dialogService = dialogService;
            this.connectivity = connectivity;
            this.settingsFacade = settingsFacade;
            this.toastService = toastService;
        }

        /// <inheritdoc/>
        public RelayCommand InitializeCommand => new RelayCommand(async () => await InitializeAsync());

        /// <inheritdoc/>
        public RelayCommand LoginCommand => new RelayCommand(async () => await LoginAsync());

        /// <inheritdoc/>
        public RelayCommand LogoutCommand => new RelayCommand(async () => await LogoutAsync());

        /// <inheritdoc/>
        public RelayCommand BackupCommand => new RelayCommand(async () => await CreateBackupAsync());

        /// <inheritdoc/>
        public RelayCommand RestoreCommand => new RelayCommand(async () => await RestoreBackupAsync());

        /// <summary>
        /// The Date when the backup was modified the last time.
        /// </summary>
        public DateTime BackupLastModified
        {
            get => backupLastModified;
            private set
            {
                if(backupLastModified == value)
                    return;
                backupLastModified = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Indicator that the app is checking if backups available.
        /// </summary>
        public bool IsLoadingBackupAvailability
        {
            get => isLoadingBackupAvailability;
            private set
            {
                if(isLoadingBackupAvailability == value)
                    return;
                isLoadingBackupAvailability = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Indicator that the user logged in to the backup service.
        /// </summary>
        public bool IsLoggedIn => settingsFacade.IsLoggedInToBackupService;

        /// <summary>
        /// Indicates if a backup is available for restore.
        /// </summary>
        public bool BackupAvailable
        {
            get => backupAvailable;
            private set
            {
                if(backupAvailable == value)
                    return;
                backupAvailable = value;
                RaisePropertyChanged();
            }
        }

        /// <inheritdoc/>
        public bool IsAutoBackupEnabled
        {
            get => settingsFacade.IsBackupAutouploadEnabled;
            set
            {
                if(settingsFacade.IsBackupAutouploadEnabled == value)
                    return;
                settingsFacade.IsBackupAutouploadEnabled = value;
                RaisePropertyChanged();
            }
        }

        private async Task InitializeAsync()
        {
            await LoadedAsync();
        }

        private async Task LoadedAsync()
        {
            if(!IsLoggedIn)
                return;

            if(!connectivity.IsConnected)
                await dialogService.ShowMessageAsync(Strings.NoNetworkTitle, Strings.NoNetworkMessage);

            IsLoadingBackupAvailability = true;
            try
            {
                BackupAvailable = await backupService.IsBackupExistingAsync();
                BackupLastModified = await backupService.GetBackupDateAsync();
            }
            catch(BackupAuthenticationFailedException ex)
            {
                logger.Error(ex, "Issue during Login process.");
                await backupService.LogoutAsync();
                await dialogService.ShowMessageAsync(Strings.AuthenticationFailedTitle, Strings.ErrorMessageAuthenticationFailed);
            }
            catch(ServiceException ex)
            {
                if(ex.Error.Code == "4f37.717b")
                {
                    await backupService.LogoutAsync();
                    await dialogService.ShowMessageAsync(Strings.AuthenticationFailedTitle, Strings.ErrorMessageAuthenticationFailed);
                }

                logger.Error(ex, "Issue on loading backup view.");
            }

            IsLoadingBackupAvailability = false;
        }

        private async Task LoginAsync()
        {
            if(!connectivity.IsConnected)
            {
                logger.Info("Tried to log in, but device isn't connected to the internet.");
                await dialogService.ShowMessageAsync(Strings.NoNetworkTitle, Strings.NoNetworkMessage);
            }

            try
            {
                await backupService.LoginAsync();
            }
            catch(BackupOperationCanceledException)
            {
                await dialogService.ShowMessageAsync(Strings.CanceledTitle, Strings.LoginCanceledMessage);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Login Failed.");
                await dialogService.ShowMessageAsync(Strings.LoginFailedTitle, string.Format(Strings.UnknownErrorMessage, ex.Message));
            }

            RaisePropertyChanged(nameof(IsLoggedIn));
            await LoadedAsync();
        }

        private async Task LogoutAsync()
        {
            try
            {
                await backupService.LogoutAsync();
            }
            catch(BackupOperationCanceledException)
            {
                await dialogService.ShowMessageAsync(Strings.CanceledTitle, Strings.LogoutCanceledMessage);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Logout Failed.");
                await dialogService.ShowMessageAsync(Strings.GeneralErrorTitle, ex.Message);
            }

            // ReSharper disable once ExplicitCallerInfoArgument
            RaisePropertyChanged(nameof(IsLoggedIn));
        }

        private async Task CreateBackupAsync()
        {
            if(!await ShowOverwriteBackupInfoAsync())
                return;

            await dialogService.ShowLoadingDialogAsync();

            try
            {
                await backupService.UploadBackupAsync(BackupMode.Manual);
                toastService.ShowToast(Strings.BackupCreatedMessage);

                BackupLastModified = DateTime.Now;
            }
            catch(BackupOperationCanceledException)
            {
                await dialogService.ShowMessageAsync(Strings.CanceledTitle, Strings.UploadBackupCanceledMessage);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Create Backup failed.");
                await dialogService.ShowMessageAsync(Strings.BackupFailedTitle, ex.Message);
            }

            await dialogService.HideLoadingDialogAsync();
        }

        private async Task RestoreBackupAsync()
        {
            if(!await ShowOverwriteDataInfoAsync())
                return;

            await dialogService.ShowLoadingDialogAsync();
            DateTime backupDate = await backupService.GetBackupDateAsync();
            if(settingsFacade.LastDatabaseUpdate > backupDate && !await ShowForceOverrideConfirmationAsync())
            {
                logger.Info("Restore Backup canceled by the user due to newer local data.");
                return;
            }

            await dialogService.ShowLoadingDialogAsync();

            try
            {
                await backupService.RestoreBackupAsync(BackupMode.Manual);
                toastService.ShowToast(Strings.BackupRestoredMessage);
            }
            catch(BackupOperationCanceledException)
            {
                logger.Info("Restoring the backup was canceled by the user.");
                await dialogService.ShowMessageAsync(Strings.CanceledTitle, Strings.RestoreBackupCanceledMessage);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Restore Backup failed.");
                await dialogService.ShowMessageAsync(Strings.BackupFailedTitle, ex.Message);
            }

            await dialogService.HideLoadingDialogAsync();
        }

        private async Task<bool> ShowOverwriteBackupInfoAsync()
            => await dialogService.ShowConfirmMessageAsync(Strings.OverwriteTitle,
                                                           Strings.OverwriteBackupMessage,
                                                           Strings.YesLabel,
                                                           Strings.NoLabel);

        private async Task<bool> ShowOverwriteDataInfoAsync()
            => await dialogService.ShowConfirmMessageAsync(Strings.OverwriteTitle,
                                                           Strings.OverwriteDataMessage,
                                                           Strings.YesLabel,
                                                           Strings.NoLabel);


        private async Task<bool> ShowForceOverrideConfirmationAsync()
            => await dialogService.ShowConfirmMessageAsync(Strings.ForceOverrideBackupTitle,
                                                            Strings.ForceOverrideBackupMessage,
                                                            Strings.YesLabel,
                                                            Strings.NoLabel);
    }
}
