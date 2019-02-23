using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Graph;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.BusinessLogic.Extensions;
using MoneyFox.BusinessLogic.Interfaces;
using MoneyFox.DataLayer;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Exceptions;
using MvvmCross.Plugin.File;

namespace MoneyFox.BusinessLogic.Backup
{
    public class BackupManager : IBackupManager, IDisposable
    {
        private readonly ICloudBackupService cloudBackupService;

        private readonly IMvxFileStore fileStore;
        private readonly IConnectivityAdapter connectivity;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public BackupManager(ICloudBackupService cloudBackupService,
            IMvxFileStore fileStore,
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
                await cloudBackupService.Login()
                                        .ConfigureAwait(true);
            }
            catch (BackupAuthenticationFailedException ex)
            {
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
                await cloudBackupService.Logout()
                                        .ConfigureAwait(true);
            } 
            catch (BackupAuthenticationFailedException ex)
            {
                return OperationResult.Failed(ex);
            }

            return OperationResult.Succeeded();
        }
        
        /// <inheritdoc />
        public async Task<DateTime> GetBackupDate()
        {
            if (!connectivity.IsConnected) return DateTime.MinValue;

            var date = await cloudBackupService.GetBackupDate()
                                               .ConfigureAwait(true);
            return date.ToLocalTime();
        }

        /// <inheritdoc />
        public async Task<bool> IsBackupExisting()
        {
            if (!connectivity.IsConnected) return false;

            var files = await cloudBackupService.GetFileNames()
                                                .ConfigureAwait(true);
            return files != null && files.Any();
        }

        /// <inheritdoc />
        public async Task<OperationResult> EnqueueBackupTask(int attempts = 0)
        {
            if (!connectivity.IsConnected)
                return OperationResult.Failed(new NetworkConnectionException());

            if ( attempts < ServiceConstants.SYNC_ATTEMPTS)
            {
                await semaphoreSlim.WaitAsync(ServiceConstants.BACKUP_OPERATION_TIMEOUT, cancellationTokenSource.Token)
                                   .ConfigureAwait(true);
                try
                {
                    if (await CreateNewBackup().ConfigureAwait(true))
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
                    await Task.Delay(ServiceConstants.BACKUP_REPEAT_DELAY)
                              .ConfigureAwait(true);
                    await EnqueueBackupTask(attempts + 1).ConfigureAwait(true);
                    Crashes.TrackError(ex);
                }
                catch (BackupAuthenticationFailedException ex)
                {
                    await Logout().ConfigureAwait(true);
                    OperationResult.Failed(ex);
                    Crashes.TrackError(ex);
                }
                catch (ServiceException ex)
                {
                    await Logout().ConfigureAwait(true);
                    OperationResult.Failed(ex);
                    Crashes.TrackError(ex);
                }
                catch(Exception ex)
                {
                    OperationResult.Failed(ex);
                    Crashes.TrackError(ex);
                }
            }
            return OperationResult.Failed("Too many attempts.");
        }

        /// <inheritdoc />
        public async Task<OperationResult> RestoreBackup(DateTime lastDatabaseUpdateTimestamp)
        {
            if (!connectivity.IsConnected)
                return OperationResult.Failed(new NetworkConnectionException());

            try
            {
                bool backupRestored = false;
                var backupDate = await GetBackupDate().ConfigureAwait(true);
                if (backupDate > lastDatabaseUpdateTimestamp)
                {
                    backupRestored = true;
                    await DownloadBackup().ConfigureAwait(true);
                }

                Analytics.TrackEvent("Backup Sync", new Dictionary<string, string>
                    {
                        { "Backup Restored? " , backupRestored.ToString(CultureInfo.InvariantCulture)},
                        { "Backup Date: " , backupDate.ToLongDateString()},
                    });
            } 
            catch (BackupAuthenticationFailedException ex)
            {
                await Logout().ConfigureAwait(true);
                OperationResult.Failed(ex);
                Crashes.TrackError(ex);
            } catch (ServiceException ex)
            {
                await Logout().ConfigureAwait(true);
                OperationResult.Failed(ex);
                Crashes.TrackError(ex);
            } 
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                OperationResult.Failed(ex);
            }
            return OperationResult.Succeeded();
        }

        private async Task<bool> CreateNewBackup()
        {
            if (!connectivity.IsConnected) throw new NetworkConnectionException();

            using (var dbStream = fileStore.OpenRead(DatabaseConstants.DB_NAME))
            {
                return await cloudBackupService.Upload(dbStream)
                                               .ConfigureAwait(true);
            }
        }

        private async Task DownloadBackup()
        {
            if (!connectivity.IsConnected) return;

            var backups = await cloudBackupService.GetFileNames()
                                                  .ConfigureAwait(true);

            if (backups.Contains(DatabaseConstants.BACKUP_NAME))
            {
                var backupStream = await cloudBackupService.Restore(DatabaseConstants.BACKUP_NAME, DatabaseConstants.BACKUP_NAME)
                                                           .ConfigureAwait(true);
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