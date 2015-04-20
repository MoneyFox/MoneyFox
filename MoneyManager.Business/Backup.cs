using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI.Popups;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Exceptions;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business {
    public class Backup {
        private readonly IBackupService _backupService;
        private string _creationDate;

        public Backup(IBackupService backupService) {
            _backupService = backupService;
        }

        /// <summary>
        ///     Prompts a login screen to the user.
        /// </summary>
        /// <exception cref="ConnectionException">Is thrown if the user couldn't be logged in.</exception>
        public void Login() {
            try {
                _backupService.Login();
            }
            catch (Exception ex) {
                InsightHelper.Report(ex);
                throw new ConnectionException(Translation.GetTranslation("LoginFailedMessage"), ex);
            }
        }

        /// <summary>
        ///     Upload a copy of the current database
        /// </summary>
        public async void UploadBackup() {
            if (!await ShowOverwriteInfo()) {
                return;
            }

            try {
                await _backupService.Upload();
            }
            catch (Exception ex) {
                InsightHelper.Report(ex);
                throw new BackupException(Translation.GetTranslation("BackupFailedMessage"), ex);
            }
        }

        private async Task<bool> ShowOverwriteInfo() {
            if (!string.IsNullOrEmpty(_creationDate)) {
                var dialog = new MessageDialog(Translation.GetTranslation("OverwriteBackupMessage"),
                    Translation.GetTranslation("OverwriteBackup"));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("YesLabel")));
                dialog.Commands.Add(new UICommand(Translation.GetTranslation("NoLabel")));

                var result = await dialog.ShowAsync();

                return result.Label == Translation.GetTranslation("YesLabel");
            }
            return true;
        }

        /// <summary>
        ///     Returns the creationdate of the last backup in a proper format
        /// </summary>
        /// <returns>Formatted Datestring.</returns>
        public async Task<string> GetCreationDateLastBackup() {
            var date = await _backupService.GetLastCreationDate();
            return
                _creationDate = date.ToString("f", new CultureInfo(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
        }
    }
}