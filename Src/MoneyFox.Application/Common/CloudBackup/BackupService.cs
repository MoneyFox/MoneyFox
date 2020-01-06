using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Domain.Exceptions;
using NLog;
using MoneyFox.Application.Common.Extensions;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.FileStore;
using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Application.Common.CloudBackup
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
        Task RestoreBackupAsync(BackupMode backupMode = BackupMode.Automatic);

        /// <summary>
        ///     Enqueues a new backup task.
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

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public BackupService(ICloudBackupService cloudBackupService,
                             IFileStore fileStore,
                             ISettingsFacade settingsFacade,
                             IConnectivityAdapter connectivity,
                             IContextAdapter contextAdapter)
        {
            this.cloudBackupService = cloudBackupService;
            this.fileStore = fileStore;
            this.settingsFacade = settingsFacade;
            this.connectivity = connectivity;
            this.contextAdapter = contextAdapter;
        }

        public async Task LoginAsync()
        {
            if (!connectivity.IsConnected) throw new NetworkConnectionException();

            await cloudBackupService.LoginAsync();

            settingsFacade.IsLoggedInToBackupService = true;
            settingsFacade.IsBackupAutouploadEnabled = true;
        }

        public async Task LogoutAsync()
        {
            if(!connectivity.IsConnected)
            {
                throw new NetworkConnectionException();
            }

            await cloudBackupService.LogoutAsync();

            settingsFacade.IsLoggedInToBackupService = false;
            settingsFacade.IsBackupAutouploadEnabled = false;
        }

        public async Task<bool> IsBackupExistingAsync()
        {
            if (!connectivity.IsConnected) return false;

            List<string> files = await cloudBackupService.GetFileNamesAsync();
            return files != null && files.Any();
        }

        public async Task<DateTime> GetBackupDateAsync()
        {
            if(!connectivity.IsConnected)
            {
                return DateTime.MinValue;
            }

            DateTime date = await cloudBackupService.GetBackupDateAsync();
            return date.ToLocalTime();
        }

        public async Task RestoreBackupAsync(BackupMode backupMode = BackupMode.Automatic)
        {
            if (backupMode == BackupMode.Automatic && !settingsFacade.IsBackupAutouploadEnabled)
            {
                return;
            }

            if (!connectivity.IsConnected)
            {
                throw new NetworkConnectionException();
            }

            await DownloadBackupAsync();
            settingsFacade.LastDatabaseUpdate = DateTime.Now;
            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
        }

        private async Task DownloadBackupAsync()
        {
            DateTime backupDate = await GetBackupDateAsync();
            if (settingsFacade.LastDatabaseUpdate > backupDate) return;

            List<string> backups = await cloudBackupService.GetFileNamesAsync();

            if (backups.Contains(DatabaseConstants.BACKUP_NAME))
            {
                using (Stream backupStream = await cloudBackupService.RestoreAsync(DatabaseConstants.BACKUP_NAME,
                                                                                  DatabaseConstants.BACKUP_NAME))
                {
                    fileStore.WriteFile(DatabaseConstants.BACKUP_NAME, backupStream.ReadToEnd());
                }

                bool moveSucceed = fileStore.TryMove(DatabaseConstants.BACKUP_NAME,
                                                     DatabasePathHelper.GetDbPath(),
                                                     true);

                if(!moveSucceed)
                {
                    throw new BackupException("Error Moving downloaded backup file");
                }
                contextAdapter.RecreateContext();
            }
        }

        public async Task UploadBackupAsync(BackupMode backupMode = BackupMode.Automatic)
        {
            if (backupMode == BackupMode.Automatic && !settingsFacade.IsBackupAutouploadEnabled)
            {
                return;
            }

            if (!settingsFacade.IsLoggedInToBackupService) await LoginAsync();

            await EnqueueBackupTaskAsync();
            settingsFacade.LastDatabaseUpdate = DateTime.Now;
        }

        private async Task EnqueueBackupTaskAsync(int attempts = 0)
        {
            if(!connectivity.IsConnected)
            {
                throw new NetworkConnectionException();
            }

            await semaphoreSlim.WaitAsync(ServiceConstants.BACKUP_OPERATION_TIMEOUT,
                                          cancellationTokenSource.Token);
            try
            {
                if (await cloudBackupService.UploadAsync(fileStore.OpenRead(DatabasePathHelper.GetDbPath())))
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
                logManager.Error(ex, "Enqueue Backup failed.");
                await Task.Delay(ServiceConstants.BACKUP_REPEAT_DELAY);
                await EnqueueBackupTaskAsync(attempts + 1);
            }
            catch(Exception ex) when (ex is BackupAuthenticationFailedException || ex is ServiceException)
            {
                logManager.Error(ex, "Enqueue Backup failed.");
                await LogoutAsync();
                throw;
            }

            logManager.Warn("Enqueue Backup failed.");
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
