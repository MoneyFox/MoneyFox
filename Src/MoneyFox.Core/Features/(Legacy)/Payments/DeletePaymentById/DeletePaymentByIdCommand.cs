namespace MoneyFox.Core.Features._Legacy_.Payments.DeletePaymentById;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class DeletePaymentByIdCommand : IRequest
{
    public DeletePaymentByIdCommand(int paymentId, bool deleteRecurringPayment = false)
    {
        PaymentId = paymentId;
        DeleteRecurringPayment = deleteRecurringPayment;
    }

    private int PaymentId { get; }

    public bool DeleteRecurringPayment { get; set; }

    public class Handler : IRequestHandler<DeletePaymentByIdCommand>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task Handle(DeletePaymentByIdCommand request, CancellationToken cancellationToken)
        {
            var entityToDelete = await appDbContext.Payments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .SingleOrDefaultAsync(predicate: x => x.Id == request.PaymentId, cancellationToken: cancellationToken);

            if (entityToDelete == null)
            {
                return;
            }

            entityToDelete.ChargedAccount.RemovePaymentAmount(entityToDelete);
            entityToDelete.TargetAccount?.RemovePaymentAmount(entityToDelete);
            if (request.DeleteRecurringPayment && entityToDelete.IsRecurring)
            {
                var recurringTransaction = await appDbContext.RecurringTransactions.SingleAsync(
                    predicate: rt => rt.RecurringTransactionId == entityToDelete.RecurringTransactionId,
                    cancellationToken: cancellationToken);

                recurringTransaction.EndRecurrence();
            }

            appDbContext.Payments.Remove(entityToDelete);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
