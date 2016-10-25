using System;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.Connectivity;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    public class BackupViewModel : BaseViewModel
    {
        private readonly IBackupManager backupManager;
        private readonly IConnectivity connectivity;
        private readonly IDialogService dialogService;
        private readonly ISettingsManager settingsManager;

        private DateTime backupLastModified;
        private bool isLoadingBackupAvailability;
        private bool backupAvailable;

        public BackupViewModel(IBackupManager backupManager,
            IDialogService dialogService,
            IConnectivity connectivity,
            ISettingsManager settingsManager)
        {
            this.backupManager = backupManager;
            this.dialogService = dialogService;
            this.connectivity = connectivity;
            this.settingsManager = settingsManager;
        }

        /// <summary>
        ///     Prepares the View when loaded.
        /// </summary>
        public MvxCommand LoadedCommand => new MvxCommand(Loaded);

        /// <summary>
        ///     Makes the first login and sets the setting for the future navigations to this page.
        /// </summary>
        public MvxCommand LoginCommand => new MvxCommand(Login);

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
        public DateTime BackupLastModified
        {
            get { return backupLastModified; }
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
            get { return isLoadingBackupAvailability; }
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
            get { return backupAvailable; }
            private set
            {
                if (backupAvailable == value) return;
                backupAvailable = value; 
                RaisePropertyChanged();
            }
        }

        private async void Loaded()
        {
            if (!connectivity.IsConnected || !IsLoggedIn)
            {
                return;
            }

            IsLoadingBackupAvailability = true;
            BackupAvailable = await backupManager.IsBackupExisting();
            BackupLastModified = await backupManager.GetBackupDate();
            IsLoadingBackupAvailability = false;
        }

        private void Login()
        {
            backupManager.Login();
            settingsManager.IsLoggedInToBackupService = true;
            RaisePropertyChanged(nameof(IsLoggedIn));
            Loaded();
        }

        private void Logout()
        {
            backupManager.Logout();
            settingsManager.IsLoggedInToBackupService = false;
            RaisePropertyChanged(nameof(IsLoggedIn));
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