namespace MoneyFox.Core.Features.BackupUpload;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common.Settings;
using JetBrains.Annotations;
using MediatR;

public static class UploadBackup
{
    public enum UploadResult { Successful, Skipped }

    public sealed class Command : IRequest<UploadResult> { }

    [UsedImplicitly]
    public class Handler : IRequestHandler<Command, UploadResult>
    {
        private const int BACKUP_ARCHIVE_THRESHOLD = 15;

        private readonly IBackupUploadService backupUploadService;
        private readonly ISettingsFacade settingsFacade;
        private readonly ISqliteBackupService sqliteBackupService;

        public Handler(IBackupUploadService backupUploadService, ISettingsFacade settingsFacade, ISqliteBackupService sqliteBackupService)
        {
            this.backupUploadService = backupUploadService;
            this.settingsFacade = settingsFacade;
            this.sqliteBackupService = sqliteBackupService;
        }

        public async Task<UploadResult> Handle(Command request, CancellationToken cancellationToken)
        {
            if (settingsFacade.IsLoggedInToBackupService is false)
            {
                return UploadResult.Skipped;
            }

            var backupDate = await backupUploadService.GetBackupDateAsync();
            if (settingsFacade.LastDatabaseUpdate - backupDate.ToLocalTime() < TimeSpan.FromSeconds(1))
            {
                return UploadResult.Skipped;
            }

            var backupResult = sqliteBackupService.CreateBackup();
            await backupUploadService.UploadAsync(backupName: backupResult.backupName, dataToUpload: backupResult.backupAsStream);
            var currentBackupDate = await backupUploadService.GetBackupDateAsync();
            settingsFacade.LastDatabaseUpdate = currentBackupDate.ToLocalTime();
            if (await backupUploadService.GetBackupCount() >= BACKUP_ARCHIVE_THRESHOLD)
            {
                await backupUploadService.DeleteOldest();
            }

            if (File.Exists(backupResult.backupAsStream.Name))
            {
                File.Delete(backupResult.backupAsStream.Name);
            }

            return UploadResult.Successful;
        }
    }
}
