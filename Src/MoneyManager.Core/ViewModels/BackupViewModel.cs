using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation.Exceptions;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Localization;

namespace MoneyManager.Core.ViewModels
{
    public class BackupViewModel : BaseViewModel
    {
        private readonly IDialogService dialogService;
        private readonly RepositoryManager repositoryManager;

        public BackupViewModel(RepositoryManager repositoryManager,
            IBackupService backupService,
            IDialogService dialogService)
        {
            this.repositoryManager = repositoryManager;
            this.dialogService = dialogService;

            BackupService = backupService;
        }

        /// <summary>
        ///     The Backup Service for the current platform.
        ///     On Android this needs to be overwritten with an
        ///     instance with the current activity setup.
        /// </summary>
        private IBackupService BackupService { get; }

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
        ///     Indicator if something is in work.
        /// </summary>
        public bool IsLoading { get; private set; }

        private async void CreateBackup()
        {
            await Login();

            if (!await ShowOverwriteBackupInfo())
            {
                return;
            }

            IsLoading = true;
            await BackupService.Upload();
            await ShowCompletionNote();
            IsLoading = false;
        }

        private async void RestoreBackup()
        {
            await Login();

            if (!await ShowOverwriteDataInfo())
            {
                return;
            }

            IsLoading = true;

            await BackupService.Restore();
            repositoryManager.ReloadData();

            await ShowCompletionNote();
            IsLoading = false;
        }

        private async Task<bool> Login()
        {
            try
            {
                IsLoading = true;
                await BackupService.Login();
                IsLoading = false;
                return true;
            }
            catch (ConnectionException)
            {
                await dialogService.ShowMessage(Strings.LoginFailedTitle, Strings.LoginFailedMessage);
                return false;
            }
        }

        private async Task<bool> ShowOverwriteBackupInfo()
        {
            return await dialogService
                .ShowConfirmMessage(Strings.OverwriteTitle, Strings.OverwriteBackupMessage);
        }

        private async Task<bool> ShowOverwriteDataInfo()
        {
            return await dialogService
                .ShowConfirmMessage(Strings.OverwriteTitle, Strings.OverwriteBackupMessage);
        }

        private async Task ShowCompletionNote()
        {
            await dialogService.ShowMessage(Strings.SuccessTitle, Strings.TaskSuccessfulMessage);
        }
    }
}