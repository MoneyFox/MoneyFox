namespace MoneyFox.Tests.Core.ApplicationCore.UseCases.BackupUpload
{

    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using MoneyFox.Core._Pending_.Common.Facades;
    using MoneyFox.Core.Interfaces;
    using MoneyFox.Infrastructure.DbBackup;
    using NSubstitute;
    using Xunit;

    public class UploadBackupShould
    {
        [Fact]
        public async Task DoNothing_When_RemoteModificationDate_NewerThan_Local()
        {
            // Assert
            var backupService = Substitute.For<UploadBackup.IBackupServiceNEW>();
            backupService.GetBackupDateAsync().Returns(DateTime.Now.AddMinutes(-2));
            var settingsFacade = Substitute.For<ISettingsFacade>();
            settingsFacade.LastDatabaseUpdate.Returns(DateTime.Now);
            var handler = new UploadBackup.Handler(
                backupService: backupService,
                settingsFacade: settingsFacade,
                fileStore: Substitute.For<IFileStore>(),
                dbPathProvider: Substitute.For<IDbPathProvider>());

            // Act
            var command = new UploadBackup.Command();
            await handler.Handle(request: command, cancellationToken: CancellationToken.None);

            // Assert
            await backupService.Received(0).UploadAsync(Arg.Any<Stream>());
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

        public interface IBackupServiceNEW
        {
            bool IsLoggedIn { get; }

            Task<DateTime> GetBackupDateAsync();

            Task UploadAsync(Stream dataToUpload);
        }

        internal class OneDriveBackupService : IBackupServiceNEW
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

}
