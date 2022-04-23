namespace MoneyFox.Core.ApplicationCore.Domain.Events.Handler
{

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using MoneyFox.Core.ApplicationCore.UseCases.DbBackup.BackupUpload;

    internal sealed class DatabaseEntityCreatedHandler : INotificationHandler<DbEntityModifiedEvent>
    {
        private readonly ISender sender;

        public DatabaseEntityCreatedHandler(ISender sender)
        {
            this.sender = sender;
        }

        public async Task Handle(DbEntityModifiedEvent notification, CancellationToken cancellationToken)
        {
            await sender.Send(request: new UploadBackupCommand(), cancellationToken: cancellationToken);
        }
    }

}
