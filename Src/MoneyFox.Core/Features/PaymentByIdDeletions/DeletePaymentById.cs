namespace MoneyFox.Core.Features.PaymentByIdDeletions;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core.Common.Interfaces;

public static class DeletePaymentById
{
    public record Command(int PaymentId, bool DeleteRecurringPayment = false) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var paymentToDelete = await appDbContext.Payments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .SingleOrDefaultAsync(predicate: x => x.Id == command.PaymentId, cancellationToken: cancellationToken);

            if (paymentToDelete == null)
            {
                return;
            }

            paymentToDelete.ChargedAccount.RemovePaymentAmount(paymentToDelete);
            paymentToDelete.TargetAccount?.RemovePaymentAmount(paymentToDelete);
            if (command.DeleteRecurringPayment && paymentToDelete.IsRecurring)
            {
                var recurringTransaction = await appDbContext.RecurringTransactions.SingleAsync(
                    predicate: rt => rt.RecurringTransactionId == paymentToDelete.RecurringTransactionId,
                    cancellationToken: cancellationToken);

                recurringTransaction.EndRecurrence();
            }

            appDbContext.Payments.Remove(paymentToDelete);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
