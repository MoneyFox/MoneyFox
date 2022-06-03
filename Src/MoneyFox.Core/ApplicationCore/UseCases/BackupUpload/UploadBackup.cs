namespace MoneyFox.Core.ApplicationCore.UseCases.BackupUpload
{

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using _Pending_.Common.Facades;
    using Interfaces;
    using MediatR;

    public static class UploadBackup
    {
        public sealed class Command : IRequest { }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private const string BACKUP_NAME_TEMPLATE = "backupmoneyfox3_{0}.db";
            private const int BACKUP_ARCHIVE_THRESHOLD = 15;

            private readonly IBackupUploadService backupUploadService;
            private readonly ISettingsFacade settingsFacade;
            private readonly IFileStore fileStore;
            private readonly IDbPathProvider dbPathProvider;

            public Handler(IBackupUploadService backupUploadService, ISettingsFacade settingsFacade, IFileStore fileStore, IDbPathProvider dbPathProvider)
            {
                this.backupUploadService = backupUploadService;
                this.settingsFacade = settingsFacade;
                this.fileStore = fileStore;
                this.dbPathProvider = dbPathProvider;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (settingsFacade.IsLoggedInToBackupService is false)
                {
                    return Unit.Value;
                }

                var backupDate = await backupUploadService.GetBackupDateAsync();
                if (settingsFacade.LastDatabaseUpdate <= backupDate)
                {
                    return Unit.Value;
                }

                var backupName = string.Format(format: BACKUP_NAME_TEMPLATE, arg0: DateTime.UtcNow.ToString(format: "yyyy-M-d_hh-mm-ssss"));
                var dbAsStream = await fileStore.OpenReadAsync(dbPathProvider.GetDbPath());
                await backupUploadService.UploadAsync(backupName: backupName, dataToUpload: dbAsStream);
                settingsFacade.LastDatabaseUpdate = await backupUploadService.GetBackupDateAsync();
                if (await backupUploadService.GetBackupCount() >= BACKUP_ARCHIVE_THRESHOLD)
                {
                    await backupUploadService.DeleteOldest();
                }

                return Unit.Value;
            }
        }
    }

}
