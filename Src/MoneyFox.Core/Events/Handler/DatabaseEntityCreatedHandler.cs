namespace MoneyFox.Core.Events.Handler
{

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using UseCases;
    using UseCases.BackupUpload;

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
