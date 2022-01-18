using MediatR;
using MoneyFox.Core.Commands.DatabaseBackup.UploadBackup;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Events.Handlers
{
    // Todo: Challenge this
    internal sealed class DatabaseEntityCreatedHandler 
        : INotificationHandler<AccountCreatedEvent>, 
            INotificationHandler<PaymentCreatedEvent>,
            INotificationHandler<CategoryCreatedEvent>
    {
        private readonly IMediator mediator;

        public DatabaseEntityCreatedHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
        {
            await mediator.Send(new UploadBackupCommand(), cancellationToken);
        }

        public async Task Handle(PaymentCreatedEvent notification, CancellationToken cancellationToken)
        {
            await mediator.Send(new UploadBackupCommand(), cancellationToken);
        }

        public async Task Handle(CategoryCreatedEvent notification, CancellationToken cancellationToken)
        {
            await mediator.Send(new UploadBackupCommand(), cancellationToken);
        }
    }
}