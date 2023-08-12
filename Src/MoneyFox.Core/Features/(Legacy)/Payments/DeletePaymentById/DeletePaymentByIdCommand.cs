namespace MoneyFox.Core.Features._Legacy_.Payments.DeletePaymentById;

using System.Linq;
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
                .Include(x => x.RecurringPayment)
                .SingleOrDefaultAsync(predicate: x => x.Id == request.PaymentId, cancellationToken: cancellationToken);

            if (entityToDelete == null)
            {
                return;
            }

            entityToDelete.ChargedAccount.RemovePaymentAmount(entityToDelete);
            entityToDelete.TargetAccount?.RemovePaymentAmount(entityToDelete);
            if (request.DeleteRecurringPayment && entityToDelete.RecurringPayment != null)
            {
                await DeleteRecurringPaymentAsync(entityToDelete.RecurringPayment.Id);
            }

            appDbContext.Payments.Remove(entityToDelete);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task DeleteRecurringPaymentAsync(int recurringPaymentId)
        {
            var payments = await appDbContext.Payments.Where(x => x.IsRecurring).Where(x => x.RecurringPayment!.Id == recurringPaymentId).ToListAsync();
            payments.ForEach(x => x.RemoveRecurringTransaction());
            var recurringPayment = await appDbContext.RecurringPayments.FindAsync(recurringPaymentId);
            if (recurringPayment is null)
            {
                return;
            }

            appDbContext.RecurringPayments.Remove(recurringPayment);
        }
    }
}
