using System;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.Connectivity;
using MoneyFox.Shared.Exceptions;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Shared.ViewModels
{
    public class BackupViewModel : BaseViewModel
    {
        private readonly IBackupManager backupManager;
        private readonly IConnectivity connectivity;
        private readonly IDialogService dialogService;

        public BackupViewModel(IBackupManager backupManager,
            IDialogService dialogService,
            IConnectivity connectivity)
        {
            this.backupManager = backupManager;
            this.dialogService = dialogService;
            this.connectivity = connectivity;
        }

        /// <summary>
        ///     Prepares the View when loaded.
        /// </summary>
        public MvxCommand LoadedCommand => new MvxCommand(Loaded);

        /// <summary>
        ///     Logs the user in.
        /// </summary>
        public MvxCommand LoginCommand => new MvxCommand(Login);

        /// <summary>
        ///     Logout the user.
        /// </summary>
        public MvxCommand LogoutCommand => new MvxCommand(Logout);

        /// <summary>
        ///     Will create a backup of the database and upload it to onedrive
        /// </summary>
        public MvxCommand BackupCommand => new MvxCommand(CreateBackup);

        /// <summary>
        ///     Will download the database backup from onedrive and overwrite the
        ///     local database with the downloaded.
        ///     All datamodels are then reloaded.
        /// </summary>
        public MvxCommand RestoreCommand => new MvxCommand(RestoreBackup);

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     The Date when the backup was modified the last time.
        /// </summary>
        public DateTime BackupLastModified { get; private set; }

        /// <summary>
        ///     Indicator that the app is checking if backups available.
        /// </summary>
        public bool IsCheckingBackupAvailability { get; private set; }

        /// <summary>
        ///     Indicates if the Login or Logout button should be presented.
        /// </summary>
        public bool IsLoggedIn => SettingsHelper.IsLoggedInToBackupService;

        public bool BackupAvailable { get; private set; }

        public async void Login()
        {
            if (!connectivity.IsConnected)
            {
                await dialogService.ShowMessage(Strings.NoNetworkTitle, Strings.NoNetworkMessage);
            }

            dialogService.ShowLoadingDialog();
            try
            {
                await backupManager.Login();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsLoggedIn));

                BackupAvailable = await backupManager.IsBackupExisting();
                BackupLastModified = await backupManager.GetBackupDate();
                SettingsHelper.IsLoggedInToBackupService = true;
            }
            catch (BackupException)
            {
                await dialogService.ShowMessage(Strings.SomethingWentWrongTitle, Strings.AuthenticationFailedMessage);
            }
            finally
            {
                dialogService.HideLoadingDialog();
            }
        }

        public async void Logout()
        {
            if (connectivity.IsConnected)
            {
                dialogService.ShowLoadingDialog();

                await backupManager.Logout();
                // ReSharper disable once ExplicitCallerInfoArgument
                RaisePropertyChanged(nameof(IsLoggedIn));

                BackupAvailable = false;
                SettingsHelper.IsLoggedInToBackupService = false;
                BackupLastModified = new DateTime();

                dialogService.HideLoadingDialog();
            }
        }

        private async void Loaded()
        {
            if (!IsLoggedIn || !connectivity.IsConnected) return;

            IsCheckingBackupAvailability = true;
            BackupAvailable = await backupManager.IsBackupExisting();
            BackupLastModified = await backupManager.GetBackupDate();
            IsCheckingBackupAvailability = false;
        }

        private async void CreateBackup()
        {
            if (!await ShowOverwriteBackupInfo())
            {
                return;
            }

            dialogService.ShowLoadingDialog();
            await backupManager.CreateNewBackup();
            BackupLastModified = DateTime.Now;
            dialogService.HideLoadingDialog();
            await ShowCompletionNote();
        }

        private async void RestoreBackup()
        {
            if (!await ShowOverwriteDataInfo())
            {
                return;
            }

            dialogService.ShowLoadingDialog();
            await backupManager.RestoreBackup();
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