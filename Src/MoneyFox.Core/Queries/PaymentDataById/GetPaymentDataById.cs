namespace MoneyFox.Core.Queries.PaymentDataById;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core.Common.Interfaces;

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
                    .Select(
                        rt => new RecurrenceData(
                            rt.Recurrence,
                            rt.StartDate.ToDateTime(TimeOnly.MinValue),
                            rt.EndDate.HasValue ? rt.EndDate.Value.ToDateTime(TimeOnly.MinValue) : null))
                    .SingleAsync(cancellationToken);
            }

            var paymentData = await appDbContext.Payments.Where(p => p.Id == query.PaymentId)
                .Select(
                    p => new PaymentData(
                        p.Id,
                        p.Amount,
                        p.ChargedAccount.Id,
                        p.TargetAccount != null ? p.TargetAccount.Id : null,
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
