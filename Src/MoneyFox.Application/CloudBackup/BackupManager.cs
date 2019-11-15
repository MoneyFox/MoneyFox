using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using MoneyFox.Application.Adapters;
using MoneyFox.Application.Constants;
using MoneyFox.Application.Extensions;
using MoneyFox.Application.FileStore;
using MoneyFox.Application.Messages;
using MoneyFox.Domain.Exceptions;
using NLog;
using Logger = NLog.Logger;

namespace MoneyFox.Application.CloudBackup
{
    public class BackupManager : IBackupManager, IDisposable
    {
        private readonly ICloudBackupService cloudBackupService;
        private readonly IFileStore fileStore;
        private readonly IConnectivityAdapter connectivity;
        private readonly IMessenger messenger;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        public BackupManager(ICloudBackupService cloudBackupService,
                             IFileStore fileStore,
                             IConnectivityAdapter connectivity,
                             IMessenger messenger)
        {
            this.cloudBackupService = cloudBackupService;
            this.fileStore = fileStore;
            this.connectivity = connectivity;
            this.messenger = messenger;
        }

        /// <inheritdoc />
        public async Task LoginAsync()
        {
            if (!connectivity.IsConnected)
                throw new NetworkConnectionException();

            try
            {
                await cloudBackupService.LoginAsync();
            }
            catch (BackupAuthenticationFailedException ex)
            {
                logManager.Error(ex, "Login Failed.");

                throw;
            }
            catch (MsalClientException ex)
            {
                logManager.Error(ex, "Login Failed.");

                throw;
            }
        }

        /// <inheritdoc />
        public async Task LogoutAsync()
        {
            if (!connectivity.IsConnected)
                throw new NetworkConnectionException();

            try
            {
                await cloudBackupService.LogoutAsync();
            }
            catch (BackupAuthenticationFailedException ex)
            {
                logManager.Error(ex, "Logout Failed.");

                throw;
            }
        }

        /// <inheritdoc />
        public async Task<DateTime> GetBackupDateAsync()
        {
            if (!connectivity.IsConnected) return DateTime.MinValue;

            DateTime date = await cloudBackupService.GetBackupDateAsync();

            return date.ToLocalTime();
        }

        /// <inheritdoc />
        public async Task<bool> IsBackupExistingAsync()
        {
            if (!connectivity.IsConnected) return false;

            List<string> files = await cloudBackupService.GetFileNamesAsync();

            return files != null && files.Any();
        }

        /// <inheritdoc />
        public async Task EnqueueBackupTaskAsync(int attempts = 0)
        {
            if (!connectivity.IsConnected)
                throw new NetworkConnectionException();

            if (attempts < ServiceConstants.SYNC_ATTEMPTS)
            {
                await semaphoreSlim.WaitAsync(ServiceConstants.BACKUP_OPERATION_TIMEOUT, cancellationTokenSource.Token);
                try
                {
                    if (await CreateNewBackup())
                        semaphoreSlim.Release();
                    else
                        cancellationTokenSource.Cancel();
                }
                catch (OperationCanceledException ex)
                {
                    logManager.Error(ex, "Enqueue Backup failed.");
                    await Task.Delay(ServiceConstants.BACKUP_REPEAT_DELAY);
                    await EnqueueBackupTaskAsync(attempts + 1);
                }
                catch (BackupAuthenticationFailedException ex)
                {
                    logManager.Error(ex, "Enqueue Backup failed.");
                    await LogoutAsync();

                    throw;
                }
                catch (ServiceException ex)
                {
                    logManager.Error(ex, "Enqueue Backup failed.");
                    await LogoutAsync();

                    throw;
                }
                catch (Exception ex)
                {
                    logManager.Error(ex, "Enqueue Backup failed.");

                    throw;
                }
            }

            logManager.Warn("Enqueue Backup failed.");
        }

        /// <inheritdoc />
        public async Task RestoreBackupAsync()
        {
            if (!connectivity.IsConnected)
                throw new NetworkConnectionException();

            try
            {
                await DownloadBackup();
                messenger.Send(new BackupRestoredMessage());
            }
            catch (BackupAuthenticationFailedException ex)
            {
                await LogoutAsync();
                logManager.Error(ex, "Download Backup failed.");

                throw;
            }
            catch (ServiceException ex)
            {
                await LogoutAsync();
                logManager.Error(ex, "Download Backup failed.");

                throw;
            }
        }

        private async Task<bool> CreateNewBackup()
        {
            if (!connectivity.IsConnected) throw new NetworkConnectionException();

            using (Stream dbStream = fileStore.OpenRead(DatabaseConstants.DB_NAME))
            {
                return await cloudBackupService.UploadAsync(dbStream);
            }
        }

        private async Task DownloadBackup()
        {
            if (!connectivity.IsConnected) return;

            List<string> backups = await cloudBackupService.GetFileNamesAsync();

            if (backups.Contains(DatabaseConstants.BACKUP_NAME))
            {
                using (Stream backupStream = await cloudBackupService.RestoreAsync(DatabaseConstants.BACKUP_NAME, DatabaseConstants.BACKUP_NAME))
                {
                    fileStore.WriteFile(DatabaseConstants.BACKUP_NAME, backupStream.ReadToEnd());
                }

                bool moveSucceed = fileStore.TryMove(DatabaseConstants.BACKUP_NAME, DatabasePathHelper.GetDbPath(), true);

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
