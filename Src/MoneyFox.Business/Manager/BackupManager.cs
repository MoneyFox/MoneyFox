using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Business.Adapter;
using MoneyFox.Business.Extensions;
using MoneyFox.DataAccess;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Exceptions;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Plugin.File;

namespace MoneyFox.Business.Manager
{
    /// <inheritdoc />
    public class BackupManager : IBackupManager
    {
        private readonly IBackupService backupService;

        private readonly IMvxFileStore fileStore;
        private readonly ISettingsManager settingsManager;
        private readonly IConnectivityAdapter connectivity;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public BackupManager(IBackupService backupService,
            IMvxFileStore fileStore,
            ISettingsManager settingsManager,
            IConnectivityAdapter connectivity)
        {
            this.backupService = backupService;
            this.fileStore = fileStore;
            this.settingsManager = settingsManager;
            this.connectivity = connectivity;
        }

        /// <inheritdoc />
        public async Task Login()
        {
            if (!connectivity.IsConnected) return;
            await backupService.Login();
            settingsManager.IsLoggedInToBackupService = true;
            settingsManager.IsBackupAutouploadEnabled = true;
        }

        /// <inheritdoc />
        public async Task Logout()
        {
            if (!connectivity.IsConnected) return;
            await backupService.Logout();
        }

        /// <inheritdoc />
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
                catch (OperationCanceledException ex)
                {
                    await Task.Delay(ServiceConstants.BackupRepeatDelay);
                    await EnqueueBackupTask(attempts + 1);
                    Crashes.TrackError(ex);
                }
            }
        }

        /// <inheritdoc />
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
                Crashes.TrackError(ex);
            }
        }

        /// <inheritdoc />
        public async Task<DateTime> GetBackupDate()
        {
            if (!connectivity.IsConnected) return DateTime.MinValue;

            var date = await backupService.GetBackupDate();
            return date.ToLocalTime();
        }

        /// <inheritdoc />
        public async Task<bool> IsBackupExisting()
        {
            if (!connectivity.IsConnected) return false;

            var files = await backupService.GetFileNames();
            return files != null && files.Any();
        }

        /// <inheritdoc />
        public async Task<bool> CreateNewBackup()
        {
            if (!connectivity.IsConnected) throw new NetworkConnectionException();

            using (var dbstream = fileStore.OpenRead(DatabaseConstants.DB_NAME))
            {
                return await backupService.Upload(dbstream);
            }
        }

        /// <inheritdoc />
        public async Task RestoreBackup()
        {
            if (!connectivity.IsConnected) return;

            var backups = await backupService.GetFileNames();

            if (backups.Contains(DatabaseConstants.BACKUP_NAME))
            {
                var backupStream =
                    await backupService.Restore(DatabaseConstants.BACKUP_NAME, DatabaseConstants.BACKUP_NAME);
                fileStore.WriteFile(DatabaseConstants.BACKUP_NAME, backupStream.ReadToEnd());

                var moveSucceed = fileStore.TryMove(DatabaseConstants.BACKUP_NAME, ApplicationContext.DbPath, true);

                if (!moveSucceed) throw new BackupException("Error Moving downloaded backup file");
                settingsManager.LastDatabaseUpdate = DateTime.Now;
            }
        }
    }
}