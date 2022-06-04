namespace MoneyFox.Tests.Core.ApplicationCore.UseCases.BackupUpload
{

    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core._Pending_.Common.Facades;
    using MoneyFox.Core.ApplicationCore.UseCases.BackupUpload;
    using MoneyFox.Core.Interfaces;
    using NSubstitute;
    using Xunit;

    public class UploadBackupShould
    {
        private readonly IBackupUploadService backupService;
        private readonly ISettingsFacade settingsFacade;
        private readonly IFileStore fileStore;

        public UploadBackupShould()
        {
            backupService = Substitute.For<IBackupUploadService>();
            settingsFacade = Substitute.For<ISettingsFacade>();
            fileStore = Substitute.For<IFileStore>();
        }

        [Fact]
        public async Task DoNothing_When_NotLoggedIn_ToBackupLocation()
        {
            // Assert
            backupService.GetBackupDateAsync().Returns(DateTime.Today.AddMinutes(-2));
            settingsFacade.IsLoggedInToBackupService.Returns(false);
            settingsFacade.LastDatabaseUpdate.Returns(DateTime.Today);
            var handler = new UploadBackup.Handler(
                backupUploadService: backupService,
                settingsFacade: settingsFacade,
                fileStore: fileStore,
                dbPathProvider: Substitute.For<IDbPathProvider>());

            // Act
            var command = new UploadBackup.Command();
            var result = await handler.Handle(request: command, cancellationToken: CancellationToken.None);

            // Assert
            result.Should().Be(UploadBackup.UploadResult.Skipped);
        }

        [Fact]
        public async Task DoNothing_When_RemoteModificationDate_NewerThan_Local()
        {
            // Assert
            backupService.GetBackupDateAsync().Returns(DateTime.Now);
            settingsFacade.LastDatabaseUpdate.Returns(DateTime.Now.AddMinutes(-2));
            var handler = new UploadBackup.Handler(
                backupUploadService: backupService,
                settingsFacade: settingsFacade,
                fileStore: Substitute.For<IFileStore>(),
                dbPathProvider: Substitute.For<IDbPathProvider>());

            // Act
            var command = new UploadBackup.Command();
            var result = await handler.Handle(request: command, cancellationToken: CancellationToken.None);

            // Assert
            result.Should().Be(UploadBackup.UploadResult.Skipped);
        }

        [Fact]
        public async Task Upload_FileStream_When_LoggedIn_And_LocalBackup_Newer()
        {
            // Assert
            backupService.GetBackupDateAsync().Returns(returnThis: x => DateTime.Today.AddMinutes(-2), x => DateTime.Now);
            backupService.GetBackupCount().Returns(3);
            settingsFacade.IsLoggedInToBackupService.Returns(true);
            settingsFacade.LastDatabaseUpdate.Returns(DateTime.Today.AddMinutes(-1));
            fileStore.OpenReadAsync(Arg.Any<string>()).Returns(Stream.Null);
            var handler = new UploadBackup.Handler(
                backupUploadService: backupService,
                settingsFacade: settingsFacade,
                fileStore: fileStore,
                dbPathProvider: Substitute.For<IDbPathProvider>());

            // Act
            var command = new UploadBackup.Command();
            var result = await handler.Handle(request: command, cancellationToken: CancellationToken.None);

            // Assert
            await backupService.Received(1).UploadAsync(backupName: Arg.Is<string>(s => s.StartsWith("backupmoneyfox3_")), dataToUpload: Arg.Any<Stream>());
            await backupService.Received(0).DeleteOldest();
            settingsFacade.LastDatabaseUpdate.Should().BeWithin(TimeSpan.FromSeconds(3)).Before(DateTime.Now);
            result.Should().Be(UploadBackup.UploadResult.Successful);
        }

        [Fact]
        public async Task Upload_FileStream_AndDeleteOldestEntry_When_LoggedIn_And_LocalBackup_Newer_And_ArchiveThreshold_Reached()
        {
            // Assert
            backupService.GetBackupDateAsync().Returns(returnThis: x => DateTime.Today.AddMinutes(-2), x => DateTime.Now);
            backupService.GetBackupCount().Returns(15);
            settingsFacade.IsLoggedInToBackupService.Returns(true);
            settingsFacade.LastDatabaseUpdate.Returns(DateTime.Today.AddMinutes(-1));
            fileStore.OpenReadAsync(Arg.Any<string>()).Returns(Stream.Null);
            var handler = new UploadBackup.Handler(
                backupUploadService: backupService,
                settingsFacade: settingsFacade,
                fileStore: fileStore,
                dbPathProvider: Substitute.For<IDbPathProvider>());

            // Act
            var command = new UploadBackup.Command();
            var result = await handler.Handle(request: command, cancellationToken: CancellationToken.None);

            // Assert
            await backupService.Received(1).UploadAsync(backupName: Arg.Is<string>(s => s.StartsWith("backupmoneyfox3_")), dataToUpload: Arg.Any<Stream>());
            await backupService.Received(1).DeleteOldest();
            settingsFacade.LastDatabaseUpdate.Should().BeWithin(TimeSpan.FromSeconds(3)).Before(DateTime.Now);
            result.Should().Be(UploadBackup.UploadResult.Successful);
        }
    }

}
