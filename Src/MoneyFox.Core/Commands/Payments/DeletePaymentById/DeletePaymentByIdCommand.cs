namespace MoneyFox.Core.Commands.Payments.DeletePaymentById
{

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
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

        public int PaymentId { get; }

        public bool DeleteRecurringPayment { get; set; }

        public class Handler : IRequestHandler<DeletePaymentByIdCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(DeletePaymentByIdCommand request, CancellationToken cancellationToken)
            {
                var entityToDelete = await contextAdapter.Context.Payments.Include(x => x.ChargedAccount)
                    .Include(x => x.TargetAccount)
                    .Include(x => x.RecurringPayment)
                    .SingleOrDefaultAsync(predicate: x => x.Id == request.PaymentId, cancellationToken: cancellationToken);

                if (entityToDelete == null)
                {
                    throw new PaymentNotFoundException();
                }

                entityToDelete.ChargedAccount.RemovePaymentAmount(entityToDelete);
                entityToDelete.TargetAccount?.RemovePaymentAmount(entityToDelete);
                if (request.DeleteRecurringPayment && entityToDelete.RecurringPayment != null)
                {
                    await DeleteRecurringPaymentAsync(entityToDelete.RecurringPayment.Id);
                }

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);
                contextAdapter.Context.Payments.Remove(entityToDelete);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }

            private async Task DeleteRecurringPaymentAsync(int recurringPaymentId)
            {
                var payments = await contextAdapter.Context.Payments.Where(x => x.IsRecurring)
                    .Where(x => x.RecurringPayment!.Id == recurringPaymentId)
                    .ToListAsync();

                payments.ForEach(x => x.RemoveRecurringPayment());
                contextAdapter.Context.RecurringPayments.Remove(await contextAdapter.Context.RecurringPayments.FindAsync(recurringPaymentId));
            }
        }
    }

}
