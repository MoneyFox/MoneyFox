using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.ViewModels
{
    public class BackupViewModel : BaseViewModel
    {
        private readonly Backup backup;
        private readonly IDialogService dialogService;
        private readonly RepositoryManager repositoryManager;

        public BackupViewModel(Backup backup, RepositoryManager repositoryManager, IDialogService dialogService)
        {
            this.backup = backup;
            this.repositoryManager = repositoryManager;
            this.dialogService = dialogService;

            BackupCommand = new MvxCommand(CreateBackup);
            RestoreCommand = new MvxCommand(RestoreBackup);
        }

        public MvxCommand BackupCommand { get; private set; }
        public MvxCommand RestoreCommand { get; private set; }
        public bool IsLoading { get; private set; }

        private async void CreateBackup()
        {
            await Login();

            if (!await ShowOverwriteInfo())
            {
                return;
            }

            IsLoading = true;
            await backup.UploadBackup();
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

            await backup.RestoreBackup();
            repositoryManager.ReloadData();

            await ShowCompletionNote();
            IsLoading = false;
        }

        private async Task Login()
        {
            IsLoading = true;
            await backup.Login();
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