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
        private readonly Backup _backup;
        private readonly RepositoryManager _repositoryManager;

        public BackupViewModel(Backup backup, RepositoryManager repositoryManager)
        {
            _backup = backup;
            _repositoryManager = repositoryManager;

            LoadedCommand = new RelayCommand(Loaded);
            BackupCommand = new RelayCommand(CreateBackup);
            RestoreCommand = new RelayCommand(RestoreBackup);
        }

        public RelayCommand LoadedCommand { get; private set; }
        public RelayCommand BackupCommand { get; private set; }
        public RelayCommand RestoreCommand { get; private set; }
        public bool IsLoading { get; private set; }
        public string CreationDate { get; private set; }

        private async void Loaded()
        {
            CreationDate = await _backup.GetCreationDateLastBackup();
        }

        private async void CreateBackup()
        {
            Login();

            if (!await ShowOverwriteInfo())
            {
                return;
            }

            IsLoading = true;
            await _backup.UploadBackup();
            await ShowCompletionNote();
            IsLoading = false;
        }

        private async void RestoreBackup()
        {
            Login();

            if (!await ShowOverwriteInfo()) {
                return;
            }

            IsLoading = true;

            await _backup.RestoreBackup();
            _repositoryManager.ReloadData();

            await ShowCompletionNote();
            IsLoading = false;
        }
        
        private void Login() {
            IsLoading = true;
            _backup.Login();
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