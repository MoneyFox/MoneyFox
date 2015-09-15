using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation.OperationContracts;
using MoneyManager.Localization;

namespace MoneyManager.Core.ViewModels
{
    public class BackupViewModel : BaseViewModel
    {
        private readonly BackupManager backupManager;
        private readonly IDialogService dialogService;
        private readonly RepositoryManager repositoryManager;

        public BackupViewModel(BackupManager backupManager,
            RepositoryManager repositoryManager,
            IDialogService dialogService)
        {
            this.backupManager = backupManager;
            this.repositoryManager = repositoryManager;
            this.dialogService = dialogService;
        }

        public MvxCommand BackupCommand => new MvxCommand(CreateBackup);
        public MvxCommand RestoreCommand => new MvxCommand(RestoreBackup);
        public bool IsLoading { get; private set; }

        private async void CreateBackup()
        {
            await Login();

            if (!await ShowOverwriteInfo())
            {
                return;
            }

            IsLoading = true;
            await backupManager.UploadBackup();
            await ShowCompletionNote();
            IsLoading = false;
        }

        private async void RestoreBackup()
        {
            await Login();

            if (!await ShowOverwriteInfo())
            {
                return;
            }

            IsLoading = true;

            await backupManager.RestoreBackup();
            repositoryManager.ReloadData();

            await ShowCompletionNote();
            IsLoading = false;
        }

        private async Task Login()
        {
            IsLoading = true;
            await backupManager.Login();
            IsLoading = false;
        }

        private async Task<bool> ShowOverwriteInfo()
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