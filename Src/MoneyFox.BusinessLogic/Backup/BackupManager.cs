using Microsoft.AppCenter.Crashes;
using Microsoft.Graph;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.BusinessLogic.Extensions;
using MoneyFox.DataLayer;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using MoneyFox.BusinessLogic.FileStore;
using MoneyFox.Foundation;
using NLog;
using Logger = NLog.Logger;

namespace MoneyFox.BusinessLogic.Backup
{
    public class BackupManager : IBackupManager, IDisposable
    {
        private readonly ICloudBackupService cloudBackupService;

        private readonly IFileStore fileStore;
        private readonly IConnectivityAdapter connectivity;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public BackupManager(ICloudBackupService cloudBackupService,
            IFileStore fileStore,
            IConnectivityAdapter connectivity)
        {
            this.cloudBackupService = cloudBackupService;
            this.fileStore = fileStore;
            this.connectivity = connectivity;
        }

        /// <inheritdoc />
        public async Task<OperationResult> Login()
        {
            if (!connectivity.IsConnected)
                return OperationResult.Failed(new NetworkConnectionException());

            try
            {
                await cloudBackupService.Login();
            }
            catch (BackupAuthenticationFailedException ex)
            {
                logManager.Error(ex, "Login Failed.");
                return OperationResult.Failed(ex);
            }
            catch (MsalClientException ex)
            {
                logManager.Error(ex, "Login Failed.");
                return OperationResult.Failed(ex);
            }

            return OperationResult.Succeeded();
        }

        /// <inheritdoc />
        public async Task<OperationResult> Logout()
        {
            if (!connectivity.IsConnected)
                return OperationResult.Failed(new NetworkConnectionException());

            try
            {
                await cloudBackupService.Logout();
            } catch (BackupAuthenticationFailedException ex)
            {
                logManager.Error(ex, "Logout Failed.");
                return OperationResult.Failed(ex);
            }

            return OperationResult.Succeeded();
        }

        /// <inheritdoc />
        public async Task<DateTime> GetBackupDate()
        {
            if (!connectivity.IsConnected) return DateTime.MinValue;

            var date = await cloudBackupService.GetBackupDate();
            return date.ToLocalTime();
        }

        /// <inheritdoc />
        public async Task<bool> IsBackupExisting()
        {
            if (!connectivity.IsConnected) return false;

            var files = await cloudBackupService.GetFileNames();
            return files != null && files.Any();
        }

        /// <inheritdoc />
        public async Task<OperationResult> EnqueueBackupTask(int attempts = 0)
        {
            if (!connectivity.IsConnected)
                return OperationResult.Failed(new NetworkConnectionException());

            if (attempts < ServiceConstants.SYNC_ATTEMPTS)
            {
                await semaphoreSlim.WaitAsync(ServiceConstants.BACKUP_OPERATION_TIMEOUT, cancellationTokenSource.Token);
                try
                {
                    if (await CreateNewBackup())
                    {
                        semaphoreSlim.Release();
                    } else
                    {
                        cancellationTokenSource.Cancel();
                    }
                }
                catch (OperationCanceledException ex)
                {
                    logManager.Error(ex, "Enqueue Backup failed.");
                    await Task.Delay(ServiceConstants.BACKUP_REPEAT_DELAY);
                    await EnqueueBackupTask(attempts + 1);
                } 
                catch (BackupAuthenticationFailedException ex)
                {
                    logManager.Error(ex, "Enqueue Backup failed.");
                    await Logout();
                    OperationResult.Failed(ex);
                } 
                catch (ServiceException ex)
                {
                    logManager.Error(ex, "Enqueue Backup failed.");
                    await Logout();
                    OperationResult.Failed(ex);
                } 
                catch (Exception ex)
                {
                    logManager.Error(ex, "Enqueue Backup failed.");
                    return OperationResult.Failed(ex);
                }
            }
            logManager.Warn("Enqueue Backup failed.");
            return OperationResult.Failed("Enqueue Backup failed.");
        }

        /// <inheritdoc />
        public async Task<OperationResult> RestoreBackup()
        {
            if (!connectivity.IsConnected)
                return OperationResult.Failed(new NetworkConnectionException());

            try
            {
                await DownloadBackup();
            } 
            catch (BackupAuthenticationFailedException ex)
            {
                await Logout();
                OperationResult.Failed(ex);
                logManager.Error(ex, "Download Backup failed.");
            }
            catch (ServiceException ex)
            {
                await Logout();
                OperationResult.Failed(ex);
                logManager.Error(ex, "Download Backup failed.");
            }
            return OperationResult.Succeeded();
        }

        private async Task<bool> CreateNewBackup()
        {
            if (!connectivity.IsConnected) throw new NetworkConnectionException();

            using (var dbStream = fileStore.OpenRead(DatabaseConstants.DB_NAME))
            {
                return await cloudBackupService.Upload(dbStream);
            }
        }

        private async Task DownloadBackup()
        {
            if (!connectivity.IsConnected) return;

            var backups = await cloudBackupService.GetFileNames();

            if (backups.Contains(DatabaseConstants.BACKUP_NAME))
            {
                var backupStream = await cloudBackupService.Restore(DatabaseConstants.BACKUP_NAME, DatabaseConstants.BACKUP_NAME);
                fileStore.WriteFile(DatabaseConstants.BACKUP_NAME, backupStream.ReadToEnd());

                var moveSucceed = fileStore.TryMove(DatabaseConstants.BACKUP_NAME, EfCoreContext.DbPath, true);

                if (!moveSucceed) throw new BackupException("Error Moving downloaded backup file");
            }
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