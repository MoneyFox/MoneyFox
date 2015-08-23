using System;
using System.Globalization;
using System.Threading.Tasks;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Exceptions;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business
{
    public class Backup
    {
        private readonly IBackupService _backupService;

        public Backup(IBackupService backupService)
        {
            _backupService = backupService;
        }

        /// <summary>
        ///     Prompts a login screen to the user.
        /// </summary>
        /// <exception cref="ConnectionException">Is thrown if the user couldn't be logged in.</exception>
        public async Task Login()
        {
            try
            {
                await _backupService.Login();
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
                throw new ConnectionException(Strings.LoginFailedMessage, ex);
            }
        }

        /// <summary>
        ///     Upload a copy of the current database
        /// </summary>
        public async Task UploadBackup()
        {
            try
            {
                await _backupService.Upload();
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
                throw new BackupException(Strings.BackupFailedMessage, ex);
            }
        }

        /// <summary>
        ///     Restore the database backup
        /// </summary>
        /// <returns></returns>
        public async Task RestoreBackup()
        {
            try
            {
                await _backupService.Restore();
            }
            catch (Exception ex)
            {
                InsightHelper.Report(ex);
                throw new BackupException(Strings.RestoreFailedMessage, ex);
            }
        }

        /// <summary>
        ///     Returns the creationdate of the last backup in a proper format
        /// </summary>
        /// <returns>Formatted Datestring.</returns>
        public async Task<string> GetCreationDateLastBackup()
        {
            var date = await _backupService.GetLastCreationDate();
            return date.ToString("f", new CultureInfo(CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
        }
    }
}