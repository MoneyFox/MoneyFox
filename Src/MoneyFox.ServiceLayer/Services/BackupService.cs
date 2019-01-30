using System;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.ServiceLayer.Facades;

namespace MoneyFox.ServiceLayer.Services
{
    public interface IBackupService
    {
        /// <summary>
        ///     Login user.
        /// </summary>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<OperationResult> Login();

        /// <summary>
        ///     Logout user.
        /// </summary>
        Task<OperationResult> Logout();

        /// <summary>
        ///     Checks if there are backups to restore.
        /// </summary>
        /// <returns>Backups available or not.</returns>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<bool> IsBackupExisting();

        /// <summary>
        ///     Returns the date when the last backup was created.
        /// </summary>
        /// <returns>Creation date of the last backup.</returns>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<DateTime> GetBackupDate();

        /// <summary>
        ///     Restores an existing backup.
        /// </summary>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        /// <exception cref="NoBackupFoundException">Thrown when no backup with the right name is found.</exception>
        Task<OperationResult> RestoreBackup();

        /// <summary>
        ///     Enqueues a new backup task
        /// </summary>
        /// <exception cref="NetworkConnectionException">Thrown if there is no internet connection.</exception>
        Task<OperationResult> EnqueueBackupTask(int attempts = 0);

    }

    public class BackupService : IBackupService
    {
        private readonly IBackupManager backupManager;
        private readonly ISettingsFacade settingsFacade;

        public BackupService(IBackupManager backupManager, ISettingsFacade settingsFacade)
        {
            this.backupManager = backupManager;
            this.settingsFacade = settingsFacade;
        }

        public async Task<OperationResult> Login()
        {
            var result = await backupManager.Login();
            if (result.Success)
            {
                settingsFacade.IsLoggedInToBackupService = true;
                settingsFacade.IsBackupAutouploadEnabled = true;
                return OperationResult.Succeeded();
            }

            return result;
        }

        public async Task<OperationResult> Logout()
        {
            var result = await backupManager.Logout();
            if (result.Success)
            {
                settingsFacade.IsLoggedInToBackupService = false;
                settingsFacade.IsBackupAutouploadEnabled = false;
            }

            return result;
        }

        public async Task<bool> IsBackupExisting()
        {
            return await backupManager.IsBackupExisting();
        }

        public async Task<DateTime> GetBackupDate()
        {
            return await backupManager.GetBackupDate();
        }

        public async Task<OperationResult> RestoreBackup()
        {
            var result = await backupManager.RestoreBackup(settingsFacade.LastDatabaseUpdate);

            if (result.Success)
            {
                settingsFacade.LastDatabaseUpdate = DateTime.Now;
            }

            return result;
        }

        public async Task<OperationResult> EnqueueBackupTask(int attempts = 0)
        {
            if (!settingsFacade.IsBackupAutouploadEnabled) return OperationResult.Succeeded();

            if (!settingsFacade.IsLoggedInToBackupService)
            {
                var loginResult = await Login();
                if (!loginResult.Success)
                {
                    return loginResult;
                }
            }

            var result = await backupManager.EnqueueBackupTask();

            if (result.Success)
            {
                settingsFacade.LastDatabaseUpdate = DateTime.Now;
            }

            return result;
        }
    }
}
