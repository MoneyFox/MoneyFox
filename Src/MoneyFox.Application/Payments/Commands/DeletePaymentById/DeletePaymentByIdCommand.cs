using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Backup;
using MoneyFox.Application.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Payments.Commands.DeletePaymentById
{
    public class DeletePaymentByIdCommand : IRequest
    {
        public DeletePaymentByIdCommand(int paymentId)
        {
            PaymentId = paymentId;
        }

        public int PaymentId { get; }

        public class Handler : IRequestHandler<DeletePaymentByIdCommand>
        {
            private readonly IEfCoreContext context;
            private readonly IBackupService backupService;

            public Handler(IEfCoreContext context, IBackupService backupService)
            {
                this.context = context;
                this.backupService = backupService;
            }

            public async Task<Unit> Handle(DeletePaymentByIdCommand request, CancellationToken cancellationToken)
            {
                Payment entityToDelete = await context.Payments.FindAsync(request.PaymentId);

                context.Payments.Remove(entityToDelete);
                await context.SaveChangesAsync(cancellationToken);

#pragma warning disable 4014
                backupService.EnqueueBackupTaskAsync();
#pragma warning restore 4014

                return Unit.Value;
            }
        }
    }
}
