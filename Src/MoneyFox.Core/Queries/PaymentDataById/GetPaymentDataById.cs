namespace MoneyFox.Core.Queries.PaymentDataById;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetPaymentDataById
{
    public record Query(int PaymentId) : IRequest<PaymentData>;

    public class Handler : IRequestHandler<Query, PaymentData>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<PaymentData> Handle(Query query, CancellationToken cancellationToken)
        {
            var recurringTransactionId = await appDbContext.Payments.Where(p => p.Id == query.PaymentId)
                .Select(p => p.RecurringTransactionId)
                .SingleOrDefaultAsync(cancellationToken);

            RecurrenceData? recurrenceData = null;
            if (recurringTransactionId.HasValue)
            {
                recurrenceData = await appDbContext.RecurringTransactions.Where(rt => rt.RecurringTransactionId == recurringTransactionId)
                    .Select(rt => new RecurrenceData(rt.Recurrence, rt.StartDate, rt.EndDate))
                    .SingleAsync(cancellationToken);
            }

            var paymentData = await appDbContext.Payments.Where(p => p.Id == query.PaymentId)
                .Select(
                    p => new PaymentData(
                        p.Id,
                        p.Amount,
                        new(p.ChargedAccount.Id, p.ChargedAccount.Name, p.ChargedAccount.CurrentBalance),
                        p.TargetAccount != null ? new(p.TargetAccount.Id, p.TargetAccount.Name, p.TargetAccount.CurrentBalance) : null,
                        p.Category != null ? new(p.Category.Id, p.Category.Name, p.Category.RequireNote) : null,
                        p.Date,
                        p.IsCleared,
                        p.Type,
                        p.Note ?? string.Empty,
                        p.Created,
                        p.LastModified,
                        recurrenceData))
                .SingleAsync(cancellationToken);

            return paymentData;
        }
    }
}
