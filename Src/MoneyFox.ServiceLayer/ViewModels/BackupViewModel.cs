using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Microsoft.Graph;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public interface IBackupViewModel : IBaseViewModel
    {
        /// <summary>
        ///     Makes the first login and sets the setting for the future navigation to this page.
        /// </summary>
        MvxAsyncCommand LoginCommand { get; }

        /// <summary>
        ///     Logs the user out from the backup service.
        /// </summary>
        MvxAsyncCommand LogoutCommand { get; }

        /// <summary>
        ///     Will create a backup of the database and upload it to OneDrive
        /// </summary>
        MvxAsyncCommand BackupCommand { get; }

        /// <summary>
        ///     Will download the database backup from OneDrive and overwrite the
        ///     local database with the downloaded.
        ///     All data models are then reloaded.
        /// </summary>
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
        private readonly IBackupService backupService;
        private readonly IConnectivityAdapter connectivity;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;
        private bool backupAvailable;

        private DateTime backupLastModified;
        private bool isLoadingBackupAvailability;

        public BackupViewModel(IBackupService backupService,
            IDialogService dialogService,
            IConnectivityAdapter connectivity,
            ISettingsFacade settingsFacade,
            IMvxLogProvider logProvider,
            IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.backupService = backupService;
            this.dialogService = dialogService;
            this.connectivity = connectivity;
            this.settingsFacade = settingsFacade;
        }

        /// <inheritdoc />
        public MvxAsyncCommand LoginCommand => new MvxAsyncCommand(Login);

        /// <inheritdoc />
        public MvxAsyncCommand LogoutCommand => new MvxAsyncCommand(Logout);

        /// <inheritdoc />
        public MvxAsyncCommand BackupCommand => new MvxAsyncCommand(CreateBackup);

        /// <inheritdoc />
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
        public bool IsLoggedIn => settingsFacade.IsLoggedInToBackupService;

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

        public override async Task Initialize()
        {
            await Loaded();
        }

        private async Task Loaded()
        {
            if (!IsLoggedIn) return;

            if (!connectivity.IsConnected)
                await dialogService.ShowMessage(Strings.NoNetworkTitle, Strings.NoNetworkMessage);

            IsLoadingBackupAvailability = true;
            try
            {
                BackupAvailable = await backupService.IsBackupExisting();
                BackupLastModified = await backupService.GetBackupDate();
            }
            catch (BackupAuthenticationFailedException ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string> {{"Info", "Issue during Login process."}});
                await backupService.Logout();
                await dialogService.ShowMessage(Strings.AuthenticationFailedTitle,
                    Strings.ErrorMessageAuthenticationFailed);
            }
            catch (ServiceException ex)
            {
                if (ex.Error.Code == "4f37.717b")
                {
                    Crashes.TrackError(ex, new Dictionary<string, string> {{"Info", "Graph Login Exception"}});
                    await backupService.Logout();
                    await dialogService.ShowMessage(Strings.AuthenticationFailedTitle,
                        Strings.ErrorMessageAuthenticationFailed);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string> {{"Info", "Unknown Issue"}});
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

            var result = await backupService.Login();

            if (!result.Success)
            {
                await dialogService
                    .ShowMessage(Strings.LoginFailedTitle,result.Message);
            }

            // ReSharper disable once ExplicitCallerInfoArgument
            await RaisePropertyChanged(nameof(IsLoggedIn));
            await Loaded();
        }

        private async Task Logout()
        {
            var result = await backupService.Logout();

            if (!result.Success)
            {
                await dialogService
                    .ShowMessage(Strings.LoginFailedTitle, result.Message);
            }

            // ReSharper disable once ExplicitCallerInfoArgument
            await RaisePropertyChanged(nameof(IsLoggedIn));
        }

        private async Task CreateBackup()
        {
            if (!await ShowOverwriteBackupInfo()) return;

            dialogService.ShowLoadingDialog();

            var operationResult = await backupService.EnqueueBackupTask();
            if (operationResult.Success)
            {
                BackupLastModified = DateTime.Now;
            }
            else
            {
                await dialogService.ShowMessage(Strings.BackupFailedTitle, operationResult.Message);
            }

            dialogService.HideLoadingDialog();
            await ShowCompletionNote();
        }

        private async Task RestoreBackup()
        {
            if (!await ShowOverwriteDataInfo()) return;

            dialogService.ShowLoadingDialog();
            var operationResult = await backupService.RestoreBackup();
            dialogService.HideLoadingDialog();

            if (!operationResult.Success)
            {
                await dialogService.ShowMessage(Strings.BackupFailedTitle, operationResult.Message);
            }
            else
            {
                await ShowCompletionNote();
            }
        }

        private async Task<bool> ShowOverwriteBackupInfo()
        {
            return await dialogService.ShowConfirmMessage(Strings.OverwriteTitle, Strings.OverwriteBackupMessage);
        }

        private async Task<bool> ShowOverwriteDataInfo()
        {
            return await dialogService.ShowConfirmMessage(Strings.OverwriteTitle, Strings.OverwriteDataMessage);
        }

        private async Task ShowCompletionNote()
        {
            await dialogService.ShowMessage(Strings.SuccessTitle, Strings.TaskSuccessfulMessage);
        }
    }
}