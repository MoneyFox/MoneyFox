namespace MoneyFox.Core.Notifications.DatabaseChanged;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Settings;
using Features.BackupUpload;
using MediatR;

public static class DataBaseChanged
{
    public sealed class Notification : INotification;

    public class Handler(ISender sender, ISettingsFacade facade) : INotificationHandler<Notification>
    {
        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            facade.LastDatabaseUpdate = DateTime.Now;
            if (facade.IsBackupAutoUploadEnabled && facade.IsLoggedInToBackupService)
            {
                _ = await sender.Send(request: new UploadBackup.Command(), cancellationToken: cancellationToken);
            }
        }
    }
}
