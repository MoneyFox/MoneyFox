using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyManager.Foundation;

namespace MoneyManager.Business.ViewModels
{
    public class BackupViewModel : ViewModelBase
    {
        private readonly Backup _backup;

        public BackupViewModel(Backup backup)
        {
            _backup = backup;

            LoadedCommand = new RelayCommand(Loaded);
            LoginCommand = new RelayCommand(Login);
            BackupCommand = new RelayCommand(CreateBackup);
            RestoreCommand = new RelayCommand(RestoreBackup);
        }

        public RelayCommand LoadedCommand { get; private set; }
        public RelayCommand LoginCommand { get; private set; }
        public RelayCommand BackupCommand { get; private set; }
        public RelayCommand RestoreCommand { get; private set; }
        public bool IsConnected { get; private set; }
        public bool IsLoading { get; private set; }
        public string CreationDate { get; private set; }

        private async void Loaded()
        {
            //Login();
            CreationDate = await _backup.GetCreationDateLastBackup();
        }

        private async void CreateBackup()
        {
            if (!await ShowOverwriteInfo())
            {
                return;
            }

            IsLoading = true;
            await _backup.UploadBackup();
            await ShowCompletionNote();
            IsLoading = false;
        }

        private void Login()
        {
            IsLoading = true;
            _backup.Login();
            IsConnected = true;
            IsLoading = false;
        }

        private async void RestoreBackup()
        {
            if (!await ShowOverwriteInfo()) {
                return;
            }

            IsLoading = true;
            await _backup.RestoreBackup();
            await ShowCompletionNote();
            IsLoading = false;
        }

        private async Task<bool> ShowOverwriteInfo()
        {
            if (!string.IsNullOrEmpty(CreationDate))
            {
                var dialog = new MessageDialog(Translation.GetTranslation("OverwriteBackupMessage"),
                    Translation.GetTranslation("OverwriteBackup"));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("YesLabel")));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("NoLabel")));

                var result = await dialog.ShowAsync();

                return result.Label == Translation.GetTranslation("YesLabel");
            }
            return true;
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