namespace MoneyFox.Core.ApplicationCore.UseCases.DbBackup.BackupUpload
{

    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using _Pending_.Common.Facades;
    using Interfaces;
    using JetBrains.Annotations;
    using MediatR;

    [UsedImplicitly]
    internal sealed class UploadBackupCommand : IRequest
    {
        public class Handler : IRequestHandler<UploadBackupCommand, Unit>
        {
            private readonly IBackupService backupService;

            public Handler(IBackupService backupService)
            {
                this.backupService = backupService;
            }

            public async Task<Unit> Handle(UploadBackupCommand request, CancellationToken cancellationToken)
            {
                await backupService.UploadBackupAsync();

                return Unit.Value;
            }
        }
    }

    public static class UploadBackup
    {
        public sealed class Command : IRequest { }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IBackupServiceNEW backupService;
            private readonly ISettingsFacade settingsFacade;
            private readonly IFileStore fileStore;
            private readonly IDbPathProvider dbPathProvider;

            public Handler(IBackupServiceNEW backupService, ISettingsFacade settingsFacade, IFileStore fileStore, IDbPathProvider dbPathProvider)
            {
                this.backupService = backupService;
                this.settingsFacade = settingsFacade;
                this.fileStore = fileStore;
                this.dbPathProvider = dbPathProvider;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (backupService.IsLoggedIn is false)
                {
                    return Unit.Value;
                }

                var backupDate = await backupService.GetBackupDateAsync();
                if (settingsFacade.LastDatabaseUpdate > backupDate)
                {
                    var dbAsStream = await fileStore.OpenReadAsync(dbPathProvider.GetDbPath());
                    await backupService.UploadAsync(dbAsStream);
                    settingsFacade.LastDatabaseUpdate = backupDate;
                }

                return Unit.Value;
            }
        }
    }

    public interface IBackupServiceNEW
    {
        bool IsLoggedIn { get; }

        Task<DateTime> GetBackupDateAsync();

        Task UploadAsync(Stream dataToUpload);
    }

    public class OneDriveBackupService : IBackupServiceNEW
    {
        private readonly IOneDriveAuthenticationService oneDriveAuthenticationService;

        public OneDriveBackupService(IOneDriveAuthenticationService oneDriveAuthenticationService)
        {
            this.oneDriveAuthenticationService = oneDriveAuthenticationService;
        }

        public bool IsLoggedIn => oneDriveAuthenticationService.IsLoggedIn;

        public Task<DateTime> GetBackupDateAsync()
        {
            throw new NotImplementedException();
        }

        public Task UploadAsync(Stream dataToUpload)
        {
            throw new NotImplementedException();
        }
    }

}
