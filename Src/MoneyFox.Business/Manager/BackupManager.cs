using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Plugins.File;

namespace MoneyFox.Business.Manager
{
    /// <summary>
    ///     Manages the backup creation and restore process with different services.
    /// </summary>
    public class BackupManager : IBackupManager
    {
        private readonly IBackupService backupService;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly IDatabaseManager databaseManager;
        private readonly IMvxFileStore fileStore;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly ISettingsManager settingsManager;

        private bool oldBackupRestored;

        public BackupManager(IBackupService backupService,
            IMvxFileStore fileStore,
            IDatabaseManager databaseManager,
            ISettingsManager settingsManager)
        {
            this.backupService = backupService;
            this.fileStore = fileStore;
            this.databaseManager = databaseManager;
            this.settingsManager = settingsManager;
        }

        /// <summary>
        ///     Login User.
        /// </summary>
        public async Task Login()
        {
            await backupService.Login();
        }

        /// <summary>
        ///     Logout User.
        /// </summary>
        public async Task Logout()
        {
            await backupService.Logout();
        }

        /// <summary>
        ///     Enqueue a backup operation, using a semaphore to block concurrent syncs.
        ///     A sync can be attempted up to a number of times configured in ServiceConstants
        /// </summary>
        /// <param name="attempts">How many times to try syncing</param>
        public async Task EnqueueBackupTask(int attempts = 5)
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
            return (files != null) && files.Any();
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
            var backupNames = GetBackupName(await backupService.GetFileNames());
            await backupService.Restore(backupNames.Item1, backupNames.Item2);

            if (oldBackupRestored && fileStore.Exists(DatabaseConstants.DB_NAME))
            {
                fileStore.DeleteFile(DatabaseConstants.DB_NAME);
            }

            databaseManager.CreateDatabase();
            databaseManager.MigrateDatabase();

            settingsManager.LastDatabaseUpdate = DateTime.Now;
        }

        private Tuple<string, string> GetBackupName(List<string> filenames)
        {
            if (filenames.Contains(DatabaseConstants.BACKUP_NAME))
            {
                return new Tuple<string, string>(DatabaseConstants.BACKUP_NAME, DatabaseConstants.DB_NAME);
            }
            oldBackupRestored = true;
            return new Tuple<string, string>(DatabaseConstants.BACKUP_NAME_OLD, DatabaseConstants.DB_NAME_OLD);
        }
    }
}