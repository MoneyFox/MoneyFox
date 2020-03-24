using Microsoft.Graph;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.Common.Extensions;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.FileStore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Common.CloudBackup
{
    public interface IBackupService
    {
        /// <summary>
        /// Login user.
        /// </summary>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task LoginAsync();

        /// <summary>
        /// Logout user.
        /// </summary>
        Task LogoutAsync();

        /// <summary>
        /// Checks if there are backups to restore.
        /// </summary>
        /// <returns>Backups available or not.</returns>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<bool> IsBackupExistingAsync();

        /// <summary>
        /// Returns the date when the last backup was created.
        /// </summary>
        /// <returns>Creation date of the last backup.</returns>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        Task<DateTime> GetBackupDateAsync();

        /// <summary>
        /// Restores an existing backup.
        /// </summary>
        /// <exception cref="BackupAuthenticationFailedException">Thrown when the user couldn't be logged in.</exception>
        /// <exception cref="NoBackupFoundException">Thrown when no backup with the right name is found.</exception>
        Task RestoreBackupAsync(BackupMode backupMode = BackupMode.Automatic);

        /// <summary>
        /// Enqueues a new backup task.
        /// </summary>
        /// <exception cref="NetworkConnectionException">Thrown if there is no internet connection.</exception>
        Task UploadBackupAsync(BackupMode backupMode = BackupMode.Automatic);
    }

    public class BackupService : IBackupService, IDisposable
    {
        private readonly ICloudBackupService cloudBackupService;
        private readonly IFileStore fileStore;
        private readonly ISettingsFacade settingsFacade;
        private readonly IConnectivityAdapter connectivity;
        private readonly IContextAdapter contextAdapter;
        private readonly ILongRunningTaskRequester longRunningTaskRequester;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public BackupService(ICloudBackupService cloudBackupService,
                             IFileStore fileStore,
                             ISettingsFacade settingsFacade,
                             IConnectivityAdapter connectivity,
                             IContextAdapter contextAdapter,
                             ILongRunningTaskRequester longRunningTaskRequester)
        {
            this.cloudBackupService = cloudBackupService;
            this.fileStore = fileStore;
            this.settingsFacade = settingsFacade;
            this.connectivity = connectivity;
            this.contextAdapter = contextAdapter;
            this.longRunningTaskRequester = longRunningTaskRequester;
        }

        public async Task LoginAsync()
        {
            if(!connectivity.IsConnected)
                throw new NetworkConnectionException();

            await cloudBackupService.LoginAsync();

            settingsFacade.IsLoggedInToBackupService = true;
            settingsFacade.IsBackupAutouploadEnabled = true;

            logger.Info("Successfully logged in.");
        }

        public async Task LogoutAsync()
        {
            if(!connectivity.IsConnected)
                throw new NetworkConnectionException();

            await cloudBackupService.LogoutAsync();

            settingsFacade.IsLoggedInToBackupService = false;
            settingsFacade.IsBackupAutouploadEnabled = false;

            logger.Info("Successfully logged out.");
        }

        public async Task<bool> IsBackupExistingAsync()
        {
            if(!connectivity.IsConnected)
                return false;

            List<string> files = await cloudBackupService.GetFileNamesAsync();
            return files != null && files.Any();
        }

        public async Task<DateTime> GetBackupDateAsync()
        {
            if(!connectivity.IsConnected)
                return DateTime.MinValue;

            DateTime date = await cloudBackupService.GetBackupDateAsync();
            return date.ToLocalTime();
        }

        public async Task RestoreBackupAsync(BackupMode backupMode = BackupMode.Automatic)
        {
            if(backupMode == BackupMode.Automatic && !settingsFacade.IsBackupAutouploadEnabled)
            {
                logger.Info("Backup is in Automatic Mode but Auto Backup isn't enabled.");
                return;
            }

            if(!connectivity.IsConnected)
                throw new NetworkConnectionException();

            var taskId = longRunningTaskRequester.RequestLongRunning();

            await DownloadBackupAsync(backupMode);
            settingsFacade.LastDatabaseUpdate = DateTime.Now;

            longRunningTaskRequester.EndLongRunning(taskId);
        }

        private async Task DownloadBackupAsync(BackupMode backupMode)
        {
            DateTime backupDate = await GetBackupDateAsync();
            if(settingsFacade.LastDatabaseUpdate > backupDate && backupMode == BackupMode.Automatic)
            {
                logger.Info("Local backup is newer than remote. Don't download backup");
                return;
            }

            List<string> backups = await cloudBackupService.GetFileNamesAsync();

            if(backups.Contains(DatabaseConstants.BACKUP_NAME))
            {
                logger.Info("New backup found. Starting download.");
                using(Stream backupStream = await cloudBackupService.RestoreAsync(DatabaseConstants.BACKUP_NAME,
                                                                                  DatabaseConstants.BACKUP_NAME))
                {
                    fileStore.WriteFile(DatabaseConstants.BACKUP_NAME, backupStream.ReadToEnd());
                }

                logger.Info("Backup downloaded. Replace current file.");

                bool moveSucceed = fileStore.TryMove(DatabaseConstants.BACKUP_NAME,
                                                     DatabasePathHelper.GetDbPath(),
                                                     true);

                if(!moveSucceed)
                    throw new BackupException("Error Moving downloaded backup file");

                logger.Info("Recreate database context.");
                contextAdapter.RecreateContext();
            }
        }

        public async Task UploadBackupAsync(BackupMode backupMode = BackupMode.Automatic)
        {
            if(backupMode == BackupMode.Automatic && !settingsFacade.IsBackupAutouploadEnabled)
            {
                logger.Info("Backup is in Automatic Mode but Auto Backup isn't enabled.");
                return;
            }

            var taskId = longRunningTaskRequester.RequestLongRunning();

            if(!settingsFacade.IsLoggedInToBackupService)
            {
                logger.Info("Upload started, but not loggedin. Try to login.");
                await LoginAsync();
            }

            await EnqueueBackupTaskAsync();
            settingsFacade.LastDatabaseUpdate = DateTime.Now;
            longRunningTaskRequester.EndLongRunning(taskId);
        }

        private async Task EnqueueBackupTaskAsync(int attempts = 0)
        {
            if(!connectivity.IsConnected)
                throw new NetworkConnectionException();

            logger.Info("Enqueue Backup upload.");

            await semaphoreSlim.WaitAsync(ServiceConstants.BACKUP_OPERATION_TIMEOUT,
                                          cancellationTokenSource.Token);
            try
            {
                if(await cloudBackupService.UploadAsync(fileStore.OpenRead(DatabasePathHelper.GetDbPath())))
                {
                    logger.Info("Upload complete. Release Semaphore.");
                    semaphoreSlim.Release();
                }
                else
                    cancellationTokenSource.Cancel();
            }
            catch(FileNotFoundException ex)
            {
                logger.Error(ex, "Backup failed because database was not found.");
            }
            catch(OperationCanceledException ex)
            {
                logger.Error(ex, "Enqueue Backup failed.");
                await Task.Delay(ServiceConstants.BACKUP_REPEAT_DELAY);
                await EnqueueBackupTaskAsync(attempts + 1);
            }
            catch(ServiceException ex)
            {
                logger.Error(ex, "ServiceException when tried to enqueue Backup.");
                throw;
            }
            catch(BackupAuthenticationFailedException ex)
            {
                logger.Error(ex, "BackupAuthenticationFailedException when tried to enqueue Backup.");
                throw;
            }

            logger.Warn("Enqueue Backup failed.");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            cancellationTokenSource.Dispose();
            semaphoreSlim.Dispose();
        }
    }
}
