using System;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.Connectivity;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Shared.ViewModels
{
    public class BackupViewModel : BaseViewModel
    {
        private readonly IBackupManager backupManager;
        private readonly IConnectivity connectivity;
        private readonly IDialogService dialogService;

        public BackupViewModel(IBackupManager backupManager,
            IDialogService dialogService, IConnectivity connectivity)
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
        ///     The Date when the backup was modified the last time.
        /// </summary>
        public DateTime BackupLastModified { get; private set; }

        /// <summary>
        ///     Indicator if something is in work who should block the UI
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        ///     Indicator that the app is checking if backups available.
        /// </summary>
        public bool IsCheckingBackupAvailability { get; private set; }

        public bool BackupAvailable { get; private set; }

        private async void Loaded()
        {
            if (connectivity.IsConnected)
            {
                IsCheckingBackupAvailability = true;
                BackupAvailable = await backupManager.IsBackupExisting();
                BackupLastModified = await backupManager.GetBackupDate();
                IsCheckingBackupAvailability = false;
            }
            else
            {
                await dialogService.ShowMessage(Strings.NoNetworkTitle, Strings.NoNetworkMessage);
            }
        }

        private async void CreateBackup()
        {
            if (!await ShowOverwriteBackupInfo())
            {
                return;
            }

            IsLoading = true;
            await backupManager.CreateNewBackup();
            BackupLastModified = DateTime.Now;
            await ShowCompletionNote();
            IsLoading = false;
        }

        private async void RestoreBackup()
        {
            if (!await ShowOverwriteDataInfo())
            {
                return;
            }

            IsLoading = true;
            await backupManager.RestoreBackup();
            await ShowCompletionNote();
            IsLoading = false;
        }

        private async Task<bool> ShowOverwriteBackupInfo()
            => await dialogService.ShowConfirmMessage(Strings.OverwriteTitle, Strings.OverwriteBackupMessage);

        private async Task<bool> ShowOverwriteDataInfo()
            => await dialogService.ShowConfirmMessage(Strings.OverwriteTitle, Strings.OverwriteDataMessage);

        private async Task ShowCompletionNote()
            => await dialogService.ShowMessage(Strings.SuccessTitle, Strings.TaskSuccessfulMessage);
    }
}