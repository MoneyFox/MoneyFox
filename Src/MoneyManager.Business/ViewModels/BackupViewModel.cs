using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyManager.Business.Manager;
using MoneyManager.Foundation;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.ViewModels
{
    public class BackupViewModel : ViewModelBase
    {
        private readonly Backup backup;
        private readonly RepositoryManager repositoryManager;
        private readonly IDialogService dialogService;

        public BackupViewModel(Backup backup, RepositoryManager repositoryManager, IDialogService dialogService)
        {
            this.backup = backup;
            this.repositoryManager = repositoryManager;
            this.dialogService = dialogService;

            BackupCommand = new RelayCommand(CreateBackup);
            RestoreCommand = new RelayCommand(RestoreBackup);
        }

        public RelayCommand BackupCommand { get; private set; }
        public RelayCommand RestoreCommand { get; private set; }
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
                .ShowConfirmMessage(Translation.GetTranslation("OverwriteBackup"),
                    Translation.GetTranslation("OverwriteBackupMessage"));
        }

        private async Task ShowCompletionNote()
        {
            await dialogService.ShowMessage(Translation.GetTranslation("SuccessfulTitle"),
                Translation.GetTranslation("TaskSuccessfulMessage"));
        }
    }
}