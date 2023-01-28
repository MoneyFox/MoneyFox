namespace MoneyFox.Infrastructure.DbBackup.Legacy;

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Core.ApplicationCore.UseCases.DbBackup;
using Core.Common.Facades;
using Core.Common.Interfaces;
using Core.Common.Messages;
using Core.Interfaces;
using Domain.Exceptions;
using Serilog;

internal sealed class BackupService : ObservableRecipient, IBackupService, IDisposable
{
    private const string TEMP_DOWNLOAD_PATH = "backupmoneyfox3.db";
    private readonly IAppDbContext appDbContext;

    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly IConnectivityAdapter connectivity;
    private readonly IDbPathProvider dbPathProvider;
    private readonly IFileStore fileStore;

    private readonly IOneDriveBackupService oneDriveBackupService;
    private readonly SemaphoreSlim semaphoreSlim = new(initialCount: 1, maxCount: 1);
    private readonly ISettingsFacade settingsFacade;

    public BackupService(
        IOneDriveBackupService oneDriveBackupService,
        IFileStore fileStore,
        ISettingsFacade settingsFacade,
        IConnectivityAdapter connectivity,
        IDbPathProvider dbPathProvider,
        IAppDbContext appDbContext)
    {
        this.oneDriveBackupService = oneDriveBackupService;
        this.fileStore = fileStore;
        this.settingsFacade = settingsFacade;
        this.connectivity = connectivity;
        this.dbPathProvider = dbPathProvider;
        this.appDbContext = appDbContext;
    }

    public async Task LoginAsync()
    {
        if (!connectivity.IsConnected)
        {
            throw new NetworkConnectionException();
        }

        await oneDriveBackupService.LoginAsync();
        settingsFacade.IsLoggedInToBackupService = true;
    }

    public async Task LogoutAsync()
    {
        if (!connectivity.IsConnected)
        {
            throw new NetworkConnectionException();
        }

        await oneDriveBackupService.LogoutAsync();
        settingsFacade.IsLoggedInToBackupService = false;
        settingsFacade.IsBackupAutoUploadEnabled = false;
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
            return await oneDriveBackupService.GetBackupDateAsync();
        }
        catch (Exception ex) when (ex is BackupOperationCanceledException or BackupAuthenticationFailedException)
        {
            Log.Error(exception: ex, messageTemplate: "Operation canceled during get backup date. Execute logout");
            await LogoutAsync();
        }

        return DateTime.MinValue.ToLocalTime();
    }

    public async Task RestoreBackupAsync(BackupMode backupMode = BackupMode.Automatic)
    {
        if (backupMode == BackupMode.Automatic && !settingsFacade.IsBackupAutoUploadEnabled)
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
                appDbContext.MigrateDb();
                _ = Messenger.Send(new ReloadMessage());
            }
        }
        catch (BackupOperationCanceledException ex)
        {
            Log.Error(exception: ex, messageTemplate: "Operation canceled during restore backup. Execute logout");
            await LogoutAsync();
        }
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
            if (backupDate - settingsFacade.LastDatabaseUpdate < TimeSpan.FromSeconds(1) && backupMode == BackupMode.Automatic)
            {
                Log.Information("Local backup is newer than remote. Don't download backup");

                return BackupRestoreResult.Canceled;
            }

            appDbContext.ReleaseLock();
            await using (var backupStream = await oneDriveBackupService.RestoreAsync())
            {
                settingsFacade.LastDatabaseUpdate = backupDate.ToLocalTime();
                MemoryStream ms = new();
                await backupStream.CopyToAsync(ms);
                await fileStore.WriteFileAsync(path: TEMP_DOWNLOAD_PATH, contents: ms.ToArray());
            }

            var moveSucceed = await fileStore.TryMoveAsync(from: TEMP_DOWNLOAD_PATH, destination: dbPathProvider.GetDbPath(), overwrite: true);

            return !moveSucceed ? throw new BackupException("Error Moving downloaded backup file") : BackupRestoreResult.NewBackupRestored;
        }
        catch (BackupOperationCanceledException ex)
        {
            Log.Error(exception: ex, messageTemplate: "Operation canceled during restore backup. Execute logout");
            await LogoutAsync();
        }

        return BackupRestoreResult.BackupNotFound;
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
