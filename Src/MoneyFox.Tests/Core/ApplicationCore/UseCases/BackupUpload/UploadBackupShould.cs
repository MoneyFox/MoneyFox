namespace MoneyFox.Tests.Core.ApplicationCore.UseCases.BackupUpload
{

    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core._Pending_.Common.Facades;
    using MoneyFox.Core.Interfaces;
    using NSubstitute;
    using Xunit;

    public class UploadBackupShould
    {
        [Fact]
        public async Task DoNothing_When_NotLoggedIn_ToBackupLocation()
        {
            // Assert
            var backupService = Substitute.For<IBackupServiceNew>();
            backupService.GetBackupDateAsync().Returns(DateTime.Today.AddMinutes(-2));
            var settingsFacade = Substitute.For<ISettingsFacade>();
            settingsFacade.IsLoggedInToBackupService.Returns(false);
            settingsFacade.LastDatabaseUpdate.Returns(DateTime.Today);
            var fileStore = Substitute.For<IFileStore>();
            var handler = new UploadBackup.Handler(
                backupService: backupService,
                settingsFacade: settingsFacade,
                fileStore: fileStore,
                dbPathProvider: Substitute.For<IDbPathProvider>());

            // Act
            var command = new UploadBackup.Command();
            await handler.Handle(request: command, cancellationToken: CancellationToken.None);

            // Assert
            await backupService.Received(0).UploadAsync(Arg.Any<string>(), Arg.Any<Stream>());
        }

        [Fact]
        public async Task DoNothing_When_RemoteModificationDate_NewerThan_Local()
        {
            // Assert
            var backupService = Substitute.For<IBackupServiceNew>();
            backupService.GetBackupDateAsync().Returns(DateTime.Now);
            var settingsFacade = Substitute.For<ISettingsFacade>();
            settingsFacade.LastDatabaseUpdate.Returns(DateTime.Now.AddMinutes(-2));
            var handler = new UploadBackup.Handler(
                backupService: backupService,
                settingsFacade: settingsFacade,
                fileStore: Substitute.For<IFileStore>(),
                dbPathProvider: Substitute.For<IDbPathProvider>());

            // Act
            var command = new UploadBackup.Command();
            await handler.Handle(request: command, cancellationToken: CancellationToken.None);

            // Assert
            await backupService.Received(0).UploadAsync(Arg.Any<string>(), Arg.Any<Stream>());
        }

        [Fact]
        public async Task Upload_FileStream_When_LoggedIn_And_LocalBackup_Newer()
        {
            // Assert
            var backupService = Substitute.For<IBackupServiceNew>();
            backupService.GetBackupDateAsync().Returns(returnThis: x => DateTime.Today.AddMinutes(-2), x => DateTime.Now);
            backupService.GetBackupCount().Returns(3);
            var settingsFacade = Substitute.For<ISettingsFacade>();
            settingsFacade.IsLoggedInToBackupService.Returns(true);
            settingsFacade.LastDatabaseUpdate.Returns(DateTime.Today.AddMinutes(-1));
            var fileStore = Substitute.For<IFileStore>();
            fileStore.OpenReadAsync(Arg.Any<string>()).Returns(Stream.Null);
            var handler = new UploadBackup.Handler(
                backupService: backupService,
                settingsFacade: settingsFacade,
                fileStore: fileStore,
                dbPathProvider: Substitute.For<IDbPathProvider>());

            // Act
            var command = new UploadBackup.Command();
            await handler.Handle(request: command, cancellationToken: CancellationToken.None);

            // Assert
            await backupService.Received(1).UploadAsync(Arg.Is<string>(s => s.StartsWith("backupmoneyfox3_")), Arg.Any<Stream>());
            await backupService.Received(0).DeleteOldest();
            settingsFacade.LastDatabaseUpdate.Should().BeWithin(TimeSpan.FromSeconds(3)).Before(DateTime.Now);
        }

        [Fact]
        public async Task Upload_FileStream_AndDeleteOldestEntry_When_LoggedIn_And_LocalBackup_Newer_And_ArchiveThreshold_Reached()
        {
            // Assert
            var backupService = Substitute.For<IBackupServiceNew>();
            backupService.GetBackupDateAsync().Returns(returnThis: x => DateTime.Today.AddMinutes(-2), x => DateTime.Now);
            backupService.GetBackupCount().Returns(15);
            var settingsFacade = Substitute.For<ISettingsFacade>();
            settingsFacade.IsLoggedInToBackupService.Returns(true);
            settingsFacade.LastDatabaseUpdate.Returns(DateTime.Today.AddMinutes(-1));
            var fileStore = Substitute.For<IFileStore>();
            fileStore.OpenReadAsync(Arg.Any<string>()).Returns(Stream.Null);
            var handler = new UploadBackup.Handler(
                backupService: backupService,
                settingsFacade: settingsFacade,
                fileStore: fileStore,
                dbPathProvider: Substitute.For<IDbPathProvider>());

            // Act
            var command = new UploadBackup.Command();
            await handler.Handle(request: command, cancellationToken: CancellationToken.None);

            // Assert
            await backupService.Received(1).UploadAsync(Arg.Is<string>(s => s.StartsWith("backupmoneyfox3_")), Arg.Any<Stream>());
            await backupService.Received(1).DeleteOldest();
            settingsFacade.LastDatabaseUpdate.Should().BeWithin(TimeSpan.FromSeconds(3)).Before(DateTime.Now);
        }
    }

}
