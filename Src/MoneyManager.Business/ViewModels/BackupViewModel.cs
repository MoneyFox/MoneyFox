using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyManager.Business.Manager;
using MoneyManager.Foundation;

namespace MoneyManager.Business.ViewModels
{
    public class BackupViewModel : ViewModelBase
    {
        private readonly Backup backup;
        private readonly RepositoryManager repositoryManager;

        public BackupViewModel(Backup backup, RepositoryManager repositoryManager)
        {
            this.backup = backup;
            this.repositoryManager = repositoryManager;

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
            var dialog = new MessageDialog(Translation.GetTranslation("OverwriteBackupMessage"),
                Translation.GetTranslation("OverwriteBackup"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("YesLabel")));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("NoLabel")));

            var result = await dialog.ShowAsync();

            return result.Label == Translation.GetTranslation("YesLabel");
        }

        private async Task ShowCompletionNote()
        {
            var dialog = new MessageDialog(Translation.GetTranslation("TaskSuccessfulMessage"),
                Translation.GetTranslation("SuccessfulTitle"));
            dialog.Commands.Add(new UICommand(Translation.GetTranslation("OkLabel")));

            await dialog.ShowAsync();
        }
    }
}