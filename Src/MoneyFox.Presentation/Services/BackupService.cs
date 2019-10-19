using System;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.Backup;
using MoneyFox.Domain.Exceptions;
using MoneyFox.Presentation.Facades;

namespace MoneyFox.Presentation.Services
{
    public interface IBackupService
    {
        /// <summary>
        ///     Login user.
        /// </summary>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task Login();

        /// <summary>
        ///     Logout user.
        /// </summary>
        Task Logout();

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
        Task RestoreBackup();

        /// <summary>
        ///     Enqueues a new backup task.
        /// </summary>
        /// <exception cref="NetworkConnectionException">Thrown if there is no internet connection.</exception>
        Task EnqueueBackupTask(int attempts = 0);
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

        public async Task Login()
        {
            await backupManager.LoginAsync();
            settingsFacade.IsLoggedInToBackupService = true;
            settingsFacade.IsBackupAutouploadEnabled = true;
        }

        public async Task Logout()
        {
            await backupManager.LogoutAsync();
            settingsFacade.IsLoggedInToBackupService = false;
            settingsFacade.IsBackupAutouploadEnabled = false;
        }

        public async Task<bool> IsBackupExisting()
        {
            return await backupManager.IsBackupExistingAsync();
        }

        public async Task<DateTime> GetBackupDate()
        {
            return await backupManager.GetBackupDateAsync();
        }

        public async Task RestoreBackup()
        {
            await backupManager.RestoreBackupAsync();
            settingsFacade.LastDatabaseUpdate = DateTime.Now;
        }

        public async Task EnqueueBackupTask(int attempts = 0)
        {
            if (!settingsFacade.IsLoggedInToBackupService) await Login();

            await backupManager.EnqueueBackupTaskAsync();
            settingsFacade.LastDatabaseUpdate = DateTime.Now;
        }
    }
}
