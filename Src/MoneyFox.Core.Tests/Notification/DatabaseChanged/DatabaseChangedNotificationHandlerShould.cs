namespace MoneyFox.Core.Tests.Notification.DatabaseChanged;

using Core.Common.Facades;
using Core.Features.BackupUpload;
using FluentAssertions;
using MediatR;
using Notifications.DatabaseChanged;
using NSubstitute;

public class DatabaseChangedNotificationHandlerShould
{
    private readonly DataBaseChanged.Handler handler;
    private readonly ISender sender;
    private readonly ISettingsFacade settingsFacade;

    protected DatabaseChangedNotificationHandlerShould()
    {
        sender = Substitute.For<ISender>();
        settingsFacade = Substitute.For<ISettingsFacade>();
        handler = new(sender: sender, settingsFacade: settingsFacade);
    }

    public class GivenBackupAutoUploadEnabled : DatabaseChangedNotificationHandlerShould
    {
        public GivenBackupAutoUploadEnabled()
        {
            settingsFacade.IsBackupAutoUploadEnabled.Returns(true);
        }

        [Fact]
        public async Task UploadBackup_WhenLoggedIn()
        {
            // Arrange
            settingsFacade.IsLoggedInToBackupService.Returns(true);

            // Act
            var notification = new DataBaseChanged.Notification();
            await handler.Handle(notification: notification, cancellationToken: CancellationToken.None);

            // Assert
            await sender.Received().Send(request: Arg.Any<UploadBackup.Command>(), cancellationToken: CancellationToken.None);
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task Skip_UploadBackup_WhenNotLoggedIn()
        {
            // Arrange
            settingsFacade.IsLoggedInToBackupService.Returns(false);

            // Act
            var notification = new DataBaseChanged.Notification();
            await handler.Handle(notification: notification, cancellationToken: CancellationToken.None);

            // Assert
            await sender.Received(0).Send(request: Arg.Any<UploadBackup.Command>(), cancellationToken: CancellationToken.None);
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        }
    }

    public class GivenBackupAutoUploadDisabled : DatabaseChangedNotificationHandlerShould
    {
        public GivenBackupAutoUploadDisabled()
        {
            settingsFacade.IsBackupAutoUploadEnabled.Returns(false);
        }

        [Fact]
        public async Task UploadBackup_WhenLoggedIn()
        {
            // Act
            var notification = new DataBaseChanged.Notification();
            await handler.Handle(notification: notification, cancellationToken: CancellationToken.None);

            // Assert
            await sender.Received(0).Send(request: Arg.Any<UploadBackup.Command>(), cancellationToken: CancellationToken.None);
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        }
    }
}
