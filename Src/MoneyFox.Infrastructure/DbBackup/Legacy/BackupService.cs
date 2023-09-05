namespace MoneyFox.Infrastructure.DbBackup.Legacy;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Common.Settings;
using Core.Features.DbBackup;
using Core.Interfaces;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Serilog;

internal sealed class BackupService : IBackupService
{
    private readonly AppDbContext appDbContext;
    private readonly IConnectivityAdapter connectivity;
    private readonly IDbPathProvider dbPathProvider;
    private readonly IOneDriveBackupService oneDriveBackupService;
    private readonly ISettingsFacade settingsFacade;

    public BackupService(
        IOneDriveBackupService oneDriveBackupService,
        ISettingsFacade settingsFacade,
        IConnectivityAdapter connectivity,
        IDbPathProvider dbPathProvider,
        AppDbContext appDbContext)
    {
        this.oneDriveBackupService = oneDriveBackupService;
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
            }
        }
        catch (BackupOperationCanceledException ex)
        {
            Log.Error(exception: ex, messageTemplate: "Operation canceled during restore backup. Execute logout");
            await LogoutAsync();
        }
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

            var tempDownloadPath = Path.Combine(path1: Environment.GetFolderPath(Environment.SpecialFolder.Personal), path2: "money-fox_downloaded.backup" );
            await using (var backupStream = await oneDriveBackupService.RestoreAsync())
            {
                settingsFacade.LastDatabaseUpdate = backupDate.ToLocalTime();
                await WriteBackupFile(tempDownloadPath: tempDownloadPath, backupStream: backupStream);
            }

            await appDbContext.Database.CloseConnectionAsync();
            File.Move(sourceFileName: tempDownloadPath, destFileName: dbPathProvider.GetDbPath(), overwrite: true);
            await appDbContext.Database.OpenConnectionAsync();
            appDbContext.MigrateDb();

            return BackupRestoreResult.NewBackupRestored;
        }
        catch (BackupOperationCanceledException ex)
        {
            Log.Error(exception: ex, messageTemplate: "Operation canceled during restore backup. Execute logout");
            await LogoutAsync();
        }

        return BackupRestoreResult.BackupNotFound;
    }

    private static async Task WriteBackupFile(string tempDownloadPath, Stream backupStream)
    {
        await using (var fileStream = File.OpenWrite(tempDownloadPath))
        {
            await using (var binaryWriter = new BinaryWriter(fileStream))
            {
                MemoryStream ms = new();
                await backupStream.CopyToAsync(ms);
                binaryWriter.Write(ms.ToArray());
                binaryWriter.Flush();
            }
        }
    }
}
