namespace MoneyFox.Infrastructure.DbBackup.Legacy
{

    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using MoneyFox.Core._Pending_.Common.Extensions;
    using MoneyFox.Core._Pending_.Common.Facades;
    using MoneyFox.Core._Pending_.Common.Messages;
    using MoneyFox.Core.ApplicationCore.Domain.Exceptions;
    using MoneyFox.Core.ApplicationCore.UseCases.DbBackup;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Core.Interfaces;
    using MoneyFox.Core.Resources;
    using Serilog;

    internal sealed class BackupService : ObservableRecipient, IBackupService, IDisposable
    {
        private const string TEMP_DOWNLOAD_PATH = "backupmoneyfox3.db";
        private const int BACKUP_OPERATION_TIMEOUT = 10000;
        private const int BACKUP_REPEAT_DELAY = 2000;

        private readonly IOneDriveBackupService oneDriveBackupService;
        private readonly IFileStore fileStore;
        private readonly ISettingsFacade settingsFacade;
        private readonly IConnectivityAdapter connectivity;
        private readonly IContextAdapter contextAdapter;
        private readonly IToastService toastService;
        private readonly IDbPathProvider dbPathProvider;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(initialCount: 1, maxCount: 1);

        public BackupService(
            IOneDriveBackupService oneDriveBackupService,
            IFileStore fileStore,
            ISettingsFacade settingsFacade,
            IConnectivityAdapter connectivity,
            IContextAdapter contextAdapter,
            IToastService toastService,
            IDbPathProvider dbPathProvider)
        {
            this.oneDriveBackupService = oneDriveBackupService;
            this.fileStore = fileStore;
            this.settingsFacade = settingsFacade;
            this.connectivity = connectivity;
            this.contextAdapter = contextAdapter;
            this.toastService = toastService;
            this.dbPathProvider = dbPathProvider;
        }

        public async Task LoginAsync()
        {
            if (!connectivity.IsConnected)
            {
                throw new NetworkConnectionException();
            }

            await oneDriveBackupService.LoginAsync();
            settingsFacade.IsLoggedInToBackupService = true;
            await toastService.ShowToastAsync(message: Strings.LoggedInMessage, title: Strings.LoggedInTitle);
        }

        public async Task LogoutAsync()
        {
            if (!connectivity.IsConnected)
            {
                throw new NetworkConnectionException();
            }

            await oneDriveBackupService.LogoutAsync();
            settingsFacade.IsLoggedInToBackupService = false;
            settingsFacade.IsBackupAutouploadEnabled = false;
            await toastService.ShowToastAsync(message: Strings.LoggedOutMessage, title: Strings.LoggedOutTitle);
        }

        public Task<UserAccount> GetUserAccount()
        {
            if (!connectivity.IsConnected)
            {
                throw new NetworkConnectionException();
            }

            return oneDriveBackupService.GetUserAccountAsync();
        }

        public async Task<bool> IsBackupExistingAsync()
        {
            if (!connectivity.IsConnected)
            {
                return false;
            }

            var files = await oneDriveBackupService.GetFileNamesAsync();

            return files.Any();
        }

        public async Task<DateTime> GetBackupDateAsync()
        {
            if (!connectivity.IsConnected)
            {
                return DateTime.MinValue;
            }

            try
            {
                var date = await oneDriveBackupService.GetBackupDateAsync();

                return date.ToLocalTime();
            }
            catch (Exception ex) when (ex is BackupOperationCanceledException || ex is BackupAuthenticationFailedException)
            {
                Log.Error(exception: ex, messageTemplate: "Operation canceled during get backup date. Execute logout");
                await LogoutAsync();
                await toastService.ShowToastAsync(message: Strings.FailedToLoginToBackupMessage, title: Strings.FailedToLoginToBackupTitle);
            }

            return DateTime.MinValue.ToLocalTime();
        }

        public async Task RestoreBackupAsync(BackupMode backupMode = BackupMode.Automatic)
        {
            if (backupMode == BackupMode.Automatic && !settingsFacade.IsBackupAutouploadEnabled)
            {
                Log.Information("Backup is in Automatic Mode but Auto Backup isn't enabled");

                return;
            }

            if (!connectivity.IsConnected)
            {
                throw new NetworkConnectionException();
            }

            try
            {
                var result = await DownloadBackupAsync(backupMode);
                if (result == BackupRestoreResult.NewBackupRestored)
                {
                    await toastService.ShowToastAsync(Strings.BackupRestoredMessage);
                    Messenger.Send(new ReloadMessage());
                }
            }
            catch (BackupOperationCanceledException ex)
            {
                Log.Error(exception: ex, messageTemplate: "Operation canceled during restore backup. Execute logout");
                await LogoutAsync();
                await toastService.ShowToastAsync(message: Strings.FailedToLoginToBackupMessage, title: Strings.FailedToLoginToBackupTitle);
            }
        }

        public async Task UploadBackupAsync(BackupMode backupMode = BackupMode.Automatic)
        {
            if (backupMode == BackupMode.Automatic && !settingsFacade.IsBackupAutouploadEnabled)
            {
                Log.Information("Backup is in Automatic Mode but Auto Backup isn't enabled");

                return;
            }

            if (!settingsFacade.IsLoggedInToBackupService)
            {
                Log.Information("Upload started, but not logged in. Try to login");
                await LoginAsync();
            }

            await EnqueueBackupTaskAsync(backupMode);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async Task<BackupRestoreResult> DownloadBackupAsync(BackupMode backupMode)
        {
            try
            {
                var backupDate = await GetBackupDateAsync();
                if (settingsFacade.LastDatabaseUpdate > backupDate && backupMode == BackupMode.Automatic)
                {
                    Log.Information("Local backup is newer than remote. Don't download backup");

                    return BackupRestoreResult.Canceled;
                }

                await using (var backupStream = await oneDriveBackupService.RestoreAsync())
                {
                    settingsFacade.LastDatabaseUpdate = backupDate;
                    await fileStore.WriteFileAsync(path: TEMP_DOWNLOAD_PATH, contents: backupStream.ReadToEnd());
                }

                var moveSucceed = await fileStore.TryMoveAsync(from: TEMP_DOWNLOAD_PATH, destination: dbPathProvider.GetDbPath(), overwrite: true);
                if (!moveSucceed)
                {
                    throw new BackupException("Error Moving downloaded backup file");
                }

                contextAdapter.RecreateContext();

                return BackupRestoreResult.NewBackupRestored;
            }
            catch (BackupOperationCanceledException ex)
            {
                Log.Error(exception: ex, messageTemplate: "Operation canceled during restore backup. Execute logout");
                await LogoutAsync();
                await toastService.ShowToastAsync(message: Strings.FailedToLoginToBackupMessage, title: Strings.FailedToLoginToBackupTitle);
            }

            return BackupRestoreResult.BackupNotFound;
        }

        private async Task EnqueueBackupTaskAsync(BackupMode backupMode)
        {
            if (!connectivity.IsConnected)
            {
                throw new NetworkConnectionException();
            }

            var backupDate = await GetBackupDateAsync();
            await semaphoreSlim.WaitAsync(millisecondsTimeout: BACKUP_OPERATION_TIMEOUT, cancellationToken: cancellationTokenSource.Token);
            try
            {
                if (await oneDriveBackupService.UploadAsync(await fileStore.OpenReadAsync(dbPathProvider.GetDbPath())))
                {
                    semaphoreSlim.Release();
                    await toastService.ShowToastAsync(Strings.BackupCreatedMessage);
                    settingsFacade.LastDatabaseUpdate = backupDate;
                }
                else
                {
                    cancellationTokenSource.Cancel();
                }
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(exception: ex, messageTemplate: "Backup failed because database was not found");
            }
            catch (OperationCanceledException ex)
            {
                Log.Error(exception: ex, messageTemplate: "Enqueue Backup failed");
                await Task.Delay(BACKUP_REPEAT_DELAY);
                await EnqueueBackupTaskAsync(backupMode);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                cancellationTokenSource.Dispose();
                semaphoreSlim.Dispose();
            }
        }
    }

}
