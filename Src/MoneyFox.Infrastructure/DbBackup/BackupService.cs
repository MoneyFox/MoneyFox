using GalaSoft.MvvmLight.Messaging;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Adapters;
using MoneyFox.Application.Common.Constants;
using MoneyFox.Application.Common.Extensions;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.FileStore;
using MoneyFox.Application.Common.Helpers;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.DbBackup;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Infrastructure.DbBackup
{
    public class BackupService : IBackupService, IDisposable
    {
        private const int BACKUP_OPERATION_TIMEOUT = 10000;
        private const int BACKUP_REPEAT_DELAY = 2000;

        private readonly ICloudBackupService cloudBackupService;
        private readonly IFileStore fileStore;
        private readonly ISettingsFacade settingsFacade;
        private readonly IConnectivityAdapter connectivity;
        private readonly IContextAdapter contextAdapter;
        private readonly IMessenger messenger;
        private readonly IToastService toastService;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public UserAccount UserAccount { get; set; }

        public BackupService(ICloudBackupService cloudBackupService,
                             IFileStore fileStore,
                             ISettingsFacade settingsFacade,
                             IConnectivityAdapter connectivity,
                             IContextAdapter contextAdapter,
                             IMessenger messenger,
                             IToastService toastService)
        {
            this.cloudBackupService = cloudBackupService;
            this.fileStore = fileStore;
            this.settingsFacade = settingsFacade;
            this.connectivity = connectivity;
            this.contextAdapter = contextAdapter;
            this.messenger = messenger;
            this.toastService = toastService;
            UserAccount = cloudBackupService.UserAccount;
        }

        public async Task LoginAsync()
        {
            if(!connectivity.IsConnected)
            {
                throw new NetworkConnectionException();
            }

            await cloudBackupService.LoginAsync();
            if(cloudBackupService.UserAccount != null)
            {
                UserAccount = cloudBackupService.UserAccount.GetUserAccount();
            }

            settingsFacade.IsLoggedInToBackupService = true;
            settingsFacade.IsBackupAutouploadEnabled = true;

            await toastService.ShowToastAsync(Strings.LoggedInMessage, Strings.LoggedInTitle);

            logger.Info("Successfully logged in.");
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

            await toastService.ShowToastAsync(Strings.LoggedOutMessage, Strings.LoggedOutTitle);

            logger.Info("Successfully logged out.");
        }

        public async Task<bool> IsBackupExistingAsync()
        {
            if(!connectivity.IsConnected)
            {
                return false;
            }

            List<string> files = await cloudBackupService.GetFileNamesAsync();
            return files != null && files.Any();
        }

        public async Task<DateTime> GetBackupDateAsync()
        {
            if(!connectivity.IsConnected)
            {
                return DateTime.MinValue;
            }

            try
            {
                DateTime date = await cloudBackupService.GetBackupDateAsync();
                return date.ToLocalTime();
            }
            catch(Exception ex)
                when(ex is BackupOperationCanceledException || ex is BackupAuthenticationFailedException)
            {
                logger.Error(ex, "Operation canceled during get backup date. Execute logout");
                await LogoutAsync();
                await toastService.ShowToastAsync(Strings.FailedToLoginToBackupMessage, Strings.FailedToLoginToBackupTitle);
                Crashes.TrackError(ex);
            }
            return DateTime.MinValue.ToLocalTime();
        }

        public async Task RestoreBackupAsync(BackupMode backupMode = BackupMode.Automatic)
        {
            if(backupMode == BackupMode.Automatic && !settingsFacade.IsBackupAutouploadEnabled)
            {
                logger.Info("Backup is in Automatic Mode but Auto Backup isn't enabled.");
                return;
            }

            if(!connectivity.IsConnected)
            {
                throw new NetworkConnectionException();
            }

            try
            {
                BackupRestoreResult result = await DownloadBackupAsync(backupMode);

                if(result == BackupRestoreResult.NewBackupRestored)
                {
                    settingsFacade.LastDatabaseUpdate = DateTime.Now;

                    await toastService.ShowToastAsync(Strings.BackupRestoredMessage);
                    messenger.Send(new ReloadMessage());
                }
            }
            catch(BackupOperationCanceledException ex)
            {
                logger.Error(ex, "Operation canceled during restore backup. Execute logout");
                await LogoutAsync();
                await toastService.ShowToastAsync(Strings.FailedToLoginToBackupMessage, Strings.FailedToLoginToBackupTitle);
            }
        }

        private async Task<BackupRestoreResult> DownloadBackupAsync(BackupMode backupMode)
        {
            try
            {
                DateTime backupDate = await GetBackupDateAsync();
                if(settingsFacade.LastDatabaseUpdate > backupDate && backupMode == BackupMode.Automatic)
                {
                    logger.Info("Local backup is newer than remote. Don't download backup");
                    return BackupRestoreResult.Canceled;
                }

                List<string> backups = await cloudBackupService.GetFileNamesAsync();

                if(backups.Contains(DatabaseConstants.BACKUP_NAME))
                {
                    logger.Info("New backup found. Starting download.");
                    using(Stream backupStream = await cloudBackupService.RestoreAsync(DatabaseConstants.BACKUP_NAME,
                                                                                      DatabaseConstants.BACKUP_NAME))
                    {
                        await fileStore.WriteFileAsync(DatabaseConstants.BACKUP_NAME, backupStream.ReadToEnd());
                    }

                    logger.Info("Backup downloaded. Replace current file.");

                    bool moveSucceed = await fileStore.TryMoveAsync(DatabaseConstants.BACKUP_NAME,
                                                                    DatabasePathHelper.DbPath,
                                                                    true);

                    if(!moveSucceed)
                    {
                        throw new BackupException("Error Moving downloaded backup file");
                    }

                    logger.Info("Recreate database context.");
                    contextAdapter.RecreateContext();

                    return BackupRestoreResult.NewBackupRestored;
                }
            }
            catch(BackupOperationCanceledException ex)
            {
                logger.Error(ex, "Operation canceled during restore backup. Execute logout");
                await LogoutAsync();
                await toastService.ShowToastAsync(Strings.FailedToLoginToBackupMessage, Strings.FailedToLoginToBackupTitle);
            }

            return BackupRestoreResult.BackupNotFound;
        }

        public async Task UploadBackupAsync(BackupMode backupMode = BackupMode.Automatic)
        {
            if(backupMode == BackupMode.Automatic && !settingsFacade.IsBackupAutouploadEnabled)
            {
                logger.Info("Backup is in Automatic Mode but Auto Backup isn't enabled.");
                return;
            }

            if(!settingsFacade.IsLoggedInToBackupService)
            {
                logger.Info("Upload started, but not loggedin. Try to login.");
                await LoginAsync();
            }

            await EnqueueBackupTaskAsync();
            settingsFacade.LastDatabaseUpdate = DateTime.Now;
        }

        private async Task EnqueueBackupTaskAsync(int attempts = 0)
        {
            if(!connectivity.IsConnected)
            {
                throw new NetworkConnectionException();
            }

            logger.Info("Enqueue Backup upload.");

            await semaphoreSlim.WaitAsync(BACKUP_OPERATION_TIMEOUT,
                                          cancellationTokenSource.Token);
            try
            {
                if(await cloudBackupService.UploadAsync(await fileStore.OpenReadAsync(DatabasePathHelper.DbPath)))
                {
                    logger.Info("Upload complete. Release Semaphore.");
                    semaphoreSlim.Release();
                    await toastService.ShowToastAsync(Strings.BackupCreatedMessage);
                }
                else
                {
                    cancellationTokenSource.Cancel();
                }
            }
            catch(FileNotFoundException ex)
            {
                logger.Error(ex, "Backup failed because database was not found.");
            }
            catch(OperationCanceledException ex)
            {
                logger.Error(ex, "Enqueue Backup failed.");
                await Task.Delay(BACKUP_REPEAT_DELAY);
                await EnqueueBackupTaskAsync(attempts + 1);
            }
            catch(BackupAuthenticationFailedException ex)
            {
                logger.Error(ex, "BackupAuthenticationFailedException when tried to enqueue Backup.");
                throw;
            }
            catch(Exception ex)
            {
                logger.Error(ex, "ServiceException when tried to enqueue Backup.");
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
