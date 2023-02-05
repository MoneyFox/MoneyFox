namespace MoneyFox.Core.Features.BackupUpload;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Settings;
using Interfaces;
using MediatR;

public static class UploadBackup
{
    public enum UploadResult { Successful, Skipped }

    public sealed class Command : IRequest<UploadResult> { }

    public class Handler : IRequestHandler<Command, UploadResult>
    {
        private const string BACKUP_NAME_TEMPLATE = "backupmoneyfox3_{0}.db";
        private const int BACKUP_ARCHIVE_THRESHOLD = 15;

        private readonly IBackupUploadService backupUploadService;
        private readonly IDbPathProvider dbPathProvider;
        private readonly IFileStore fileStore;
        private readonly ISettingsFacade settingsFacade;

        public Handler(IBackupUploadService backupUploadService, ISettingsFacade settingsFacade, IFileStore fileStore, IDbPathProvider dbPathProvider)
        {
            this.backupUploadService = backupUploadService;
            this.settingsFacade = settingsFacade;
            this.fileStore = fileStore;
            this.dbPathProvider = dbPathProvider;
        }

        public async Task<UploadResult> Handle(Command request, CancellationToken cancellationToken)
        {
            if (settingsFacade.IsLoggedInToBackupService is false)
            {
                return UploadResult.Skipped;
            }

            var backupdate = await backupUploadService.GetBackupDateAsync();
            if (settingsFacade.LastDatabaseUpdate - backupdate.ToLocalTime() < TimeSpan.FromSeconds(1))
            {
                return UploadResult.Skipped;
            }

            var backupName = string.Format(format: BACKUP_NAME_TEMPLATE, arg0: DateTime.UtcNow.ToString(format: "yyyy-M-d_hh-mm-ssss"));
            var dbAsStream = await fileStore.OpenReadAsync(dbPathProvider.GetDbPath());
            await backupUploadService.UploadAsync(backupName: backupName, dataToUpload: dbAsStream);
            var currentBackupDate = await backupUploadService.GetBackupDateAsync();
            settingsFacade.LastDatabaseUpdate = currentBackupDate.ToLocalTime();
            if (await backupUploadService.GetBackupCount() >= BACKUP_ARCHIVE_THRESHOLD)
            {
                await backupUploadService.DeleteOldest();
            }

            return UploadResult.Successful;
        }
    }
}
