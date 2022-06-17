namespace MoneyFox.Tests.Notification.DatabaseChanged
{

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MediatR;
    using MoneyFox.Core._Pending_.Common.Facades;
    using MoneyFox.Core.ApplicationCore.UseCases.BackupUpload;
    using MoneyFox.Core.Notifications.DatabaseChanged;
    using NSubstitute;
    using NSubstitute.ReceivedExtensions;
    using Xunit;

    public sealed class DatabaseChangedNotificationHandlerShould
    {
        private readonly ISender sender;
        private readonly ISettingsFacade settingsFacade;
        private readonly DataBaseChanged.Handler handler;

        public DatabaseChangedNotificationHandlerShould()
        {
            sender = Substitute.For<ISender>();
            settingsFacade = Substitute.For<ISettingsFacade>();
            handler = new DataBaseChanged.Handler(sender, settingsFacade);
        }

        [Fact]
        public async Task UploadBackup_WhenLoggedIn_And_BackupAutoUpload_Enabled()
        {
            // Arrange
            settingsFacade.IsBackupAutoUploadEnabled.Returns(true);

            // Act
            var notification = new DataBaseChanged.Notification();
            await handler.Handle(notification: notification, cancellationToken: CancellationToken.None);

            // Assert
            await sender.Received().Send(Arg.Any<UploadBackup.Command>(), CancellationToken.None);
            settingsFacade.LastDatabaseUpdate.Should().BeCloseTo(nearbyTime: DateTime.Now, precision: TimeSpan.FromSeconds(5));
        }
    }

}
