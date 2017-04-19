using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cheesebaron.MvxPlugins.Connectivity;
using MoneyFox.Business.Extensions;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.File;

namespace MoneyFox.Business.Manager
{
    /// <summary>
    ///     Manages the backup creation and restore process with different services.
    /// </summary>
    public class BackupManager : IBackupManager
    {
        private readonly IBackupService backupService;

        private readonly IDatabaseManager databaseManager;
        private readonly IMvxFileStore fileStore;
        private readonly ISettingsManager settingsManager;
        private readonly IConnectivity connectivity;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public BackupManager(IBackupService backupService,
            IMvxFileStore fileStore,
            IDatabaseManager databaseManager,
            ISettingsManager settingsManager,
            IConnectivity connectivity)
        {
            this.backupService = backupService;
            this.fileStore = fileStore;
            this.databaseManager = databaseManager;
            this.settingsManager = settingsManager;
            this.connectivity = connectivity;
        }

        /// <summary>
        ///     Login User.
        /// </summary>
        public async Task Login()
        {
            if (!connectivity.IsConnected) return;
            await backupService.Login();
        }

        /// <summary>
        ///     Logout User.
        /// </summary>
        public async Task Logout()
        {
            if (!connectivity.IsConnected) return;
            await backupService.Logout();
        }

        /// <summary>
        ///     Enqueue a backup operation, using a semaphore to block concurrent syncs.
        ///     A sync can be attempted up to a number of times configured in ServiceConstants
        /// </summary>
        /// <param name="attempts">How many times of trying to sync already where made.</param>
        public async Task EnqueueBackupTask(int attempts = 0)
        {
            if (!connectivity.IsConnected) return;

            if (settingsManager.IsLoggedInToBackupService 
                    && settingsManager.IsBackupAutouploadEnabled 
                    && attempts < ServiceConstants.SyncAttempts)
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
        ///     Syncs the local database with the Backupservice and
        ///     restores it if the one on the Backupservice is newer.
        /// </summary>
        public async Task DownloadBackup()
        {
            if (!connectivity.IsConnected) return;

            try
            {
                if (!settingsManager.IsBackupAutouploadEnabled) return;

                if (await GetBackupDate() > settingsManager.LastDatabaseUpdate)
                {
                    await RestoreBackup();
                }
            }
            catch (Exception ex)
            {
                Mvx.Trace(MvxTraceLevel.Error, ex.Message);
            }
        }

        /// <summary>
        ///     Gets the backup date from the backup service.
        /// </summary>
        /// <returns>Backupdate.</returns>
        public async Task<DateTime> GetBackupDate()
        {
            if (!connectivity.IsConnected) return DateTime.MinValue;

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
            if (!connectivity.IsConnected) return false;

            var files = await backupService.GetFileNames();
            return (files != null) && files.Any();
        }

        /// <summary>
        ///     Creates a new backup date.
        /// </summary>
        /// <exception cref="NetworkConnectionException">Thrown if there is no internet connection.</exception>
        public async Task<bool> CreateNewBackup()
        {
            if (!connectivity.IsConnected) throw new NetworkConnectionException();

            using (var dbstream = fileStore.OpenRead(DatabaseConstants.DB_NAME))
            {
                return await backupService.Upload(dbstream);
            }
        }

        /// <summary>
        ///     Restores an existing backup from the backupservice.
        /// </summary>
        public async Task RestoreBackup()
        {
            if (!connectivity.IsConnected) return;

            var backupStream = await backupService.Restore(DatabaseConstants.BACKUP_NAME, DatabaseConstants.BACKUP_NAME);
            fileStore.WriteFile(DatabaseConstants.BACKUP_NAME, backupStream.ReadToEnd());

            var moveSucceed = fileStore.TryMove(DatabaseConstants.BACKUP_NAME, DatabaseConstants.DB_NAME, true);

            if (!moveSucceed) throw new BackupException("Error Moving downloaded backup file");

            databaseManager.CreateDatabase();

            settingsManager.LastDatabaseUpdate = DateTime.Now;
        }
    }
}