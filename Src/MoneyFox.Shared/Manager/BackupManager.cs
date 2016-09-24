using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoneyFox.Shared.Constants;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;

namespace MoneyFox.Shared.Manager
{
    /// <summary>
    ///     Manages the backup creation and restore process with different services.
    /// </summary>
    public class BackupManager : IBackupManager
    {
        private readonly IBackupService backupService;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly IDatabaseManager databaseManager;
        private readonly IRepositoryManager repositoryManager;

        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public BackupManager(IRepositoryManager repositoryManager,
            IBackupService backupService,
            IDatabaseManager databaseManager)
        {
            this.repositoryManager = repositoryManager;
            this.backupService = backupService;
            this.databaseManager = databaseManager;
        }

        public async Task Login()
        {
            await backupService.Login();
        }

        // TODO: Check if this is needed anywhere
        /// <summary>
        ///     Enqueue a backup operation, using a semaphore to block concurrent syncs.
        ///     A sync can be attempted up to a number of times configured in ServiceConstants
        /// </summary>
        /// <param name="attempts">How many times to try syncing</param>
        public async Task EnqueueBackupTask(int attempts)
        {
            if (attempts < ServiceConstants.SyncAttempts)
            {
                await semaphoreSlim.WaitAsync(ServiceConstants.BackupOperationTimeout, cancellationTokenSource.Token);
                try
                {
                    if (await CreateNewBackup())
                    {
                        semaphoreSlim.Release();
                    }
                    else
                    {
                        cancellationTokenSource.Cancel();
                    }
                }
                catch (OperationCanceledException)
                {
                    await Task.Delay(ServiceConstants.BackupRepeatDelay);
                    await EnqueueBackupTask(attempts + 1);
                }
            }
        }


        /// <summary>
        ///     Gets the backup date from the backup service.
        /// </summary>
        /// <returns>Backupdate.</returns>
        public async Task<DateTime> GetBackupDate()
        {
            var date = await backupService.GetBackupDate();
            return date.ToLocalTime();
        }

        /// <summary>
        ///     Checks if there are files in the backup folder. If yes it assumes that there are backups to restore.
        ///     There is no further check if the files are valid backup files or not.
        /// </summary>
        /// <returns>Backups available or not.</returns>
        public async Task<bool> IsBackupExisting()
        {
            var files = await backupService.GetFileNames();
            return files != null && files.Any();
        }

        /// <summary>
        ///     Creates a new backup date.
        /// </summary>
        public async Task<bool> CreateNewBackup()
        {
            return await backupService.Upload();
        }

        /// <summary>
        ///     Restores an existing backup from the backupservice.
        ///     If it was an old backup, it will delete the existing db an make an migration.
        ///     After the restore it will perform a reload of the data so that the cache works with the new data.
        /// </summary>
        public async Task RestoreBackup()
        {
            await backupService.Restore(DatabaseConstants.BACKUP_NAME, DatabaseConstants.DB_NAME);

            databaseManager.CreateDatabase();

            repositoryManager.ReloadData();
            SettingsHelper.LastDatabaseUpdate = DateTime.Now;
        }
    }
}