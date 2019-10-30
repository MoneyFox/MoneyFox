using System;
using System.Threading.Tasks;
using MoneyFox.BusinessLogic.Backup;
#pragma warning disable S1128 // Unused "using" should be removed
using MoneyFox.Domain.Exceptions;
#pragma warning restore S1128 // Unused "using" should be removed
using MoneyFox.Presentation.Facades;

namespace MoneyFox.Presentation.Services
{
    public interface IBackupService
    {
        /// <summary>
        ///     Login user.
        /// </summary>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task LoginAsync();

        /// <summary>
        ///     Logout user.
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        ///     Checks if there are backups to restore.
        /// </summary>
        /// <returns>Backups available or not.</returns>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<bool> IsBackupExistingAsync();

        /// <summary>
        ///     Returns the date when the last backup was created.
        /// </summary>
        /// <returns>Creation date of the last backup.</returns>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<DateTime> GetBackupDateAsync();

        /// <summary>
        ///     Restores an existing backup.
        /// </summary>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        /// <exception cref="NoBackupFoundException">Thrown when no backup with the right name is found.</exception>
        Task RestoreBackupAsync();

        /// <summary>
        ///     Enqueues a new backup task.
        /// </summary>
        /// <exception cref="NetworkConnectionException">Thrown if there is no internet connection.</exception>
        Task EnqueueBackupTaskAsync(int attempts = 0);
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

        public async Task LoginAsync()
        {
            await backupManager.LoginAsync();
            settingsFacade.IsLoggedInToBackupService = true;
            settingsFacade.IsBackupAutouploadEnabled = true;
        }

        public async Task LogoutAsync()
        {
            await backupManager.LogoutAsync();
            settingsFacade.IsLoggedInToBackupService = false;
            settingsFacade.IsBackupAutouploadEnabled = false;
        }

        public async Task<bool> IsBackupExistingAsync()
        {
            return await backupManager.IsBackupExistingAsync();
        }

        public async Task<DateTime> GetBackupDateAsync()
        {
            return await backupManager.GetBackupDateAsync();
        }

        public async Task RestoreBackupAsync()
        {
            await backupManager.RestoreBackupAsync();
            settingsFacade.LastDatabaseUpdate = DateTime.Now;
        }

        public async Task EnqueueBackupTaskAsync(int attempts = 0)
        {
            if (!settingsFacade.IsLoggedInToBackupService) await LoginAsync();

            await backupManager.EnqueueBackupTaskAsync();
            settingsFacade.LastDatabaseUpdate = DateTime.Now;
        }
    }
}
