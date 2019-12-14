using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Payments.Commands.DeletePaymentById
{
    public class DeletePaymentByIdCommand : IRequest
    {
        public DeletePaymentByIdCommand(int paymentId, bool deleteRecurringPayment = false)
        {
            PaymentId = paymentId;
            DeleteRecurringPayment = deleteRecurringPayment;
        }

        public int PaymentId { get; }
        public bool DeleteRecurringPayment { get; set; }

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
                await backupService.RestoreBackupAsync();

                Payment entityToDelete = await context.Payments.FindAsync(request.PaymentId);

                entityToDelete.ChargedAccount.RemovePaymentAmount(entityToDelete);
                entityToDelete.TargetAccount?.RemovePaymentAmount(entityToDelete);

                if (request.DeleteRecurringPayment)
                {
                    await DeleteRecurringPaymentAsync(entityToDelete.RecurringPayment.Id);
                }

                await context.SaveChangesAsync();

                context.Payments.Remove(entityToDelete);
                await context.SaveChangesAsync(cancellationToken);

#pragma warning disable 4014
                backupService.UploadBackupAsync();
#pragma warning restore 4014

                return Unit.Value;
            }

            private async Task DeleteRecurringPaymentAsync(int recurringPaymentId)
            {
                List<Payment> payments = await context.Payments
                                                      .Where(x => x.IsRecurring)
                                                      .Where(x => x.RecurringPayment.Id == recurringPaymentId)
                                                      .ToListAsync();

                payments.ForEach(x => x.RemoveRecurringPayment());
                context.RecurringPayments.Remove(await context.RecurringPayments.FindAsync(recurringPaymentId));
            }
        }
    }
}
