using MediatR;
using MoneyFox.Core.Commands.DatabaseBackup.UploadBackup;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Events.Handler
{
    internal sealed class DatabaseEntityCreatedHandler : INotificationHandler<DbEntityModifiedEvent>
    {
        private readonly ISender sender;

        public DatabaseEntityCreatedHandler(ISender sender)
        {
            this.sender = sender;
        }

        public async Task Handle(DbEntityModifiedEvent notification, CancellationToken cancellationToken)
        {
            await sender.Send(new UploadBackupCommand(), cancellationToken);
        }
    }
}