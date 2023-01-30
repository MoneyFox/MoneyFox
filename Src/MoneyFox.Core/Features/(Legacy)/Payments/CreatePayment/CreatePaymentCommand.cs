namespace MoneyFox.Core.Features._Legacy_.Payments.CreatePayment;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

public class CreatePaymentCommand : IRequest
{
    public CreatePaymentCommand(Payment paymentToSave)
    {
        PaymentToSave = paymentToSave;
    }

    private Payment PaymentToSave { get; }

    public class Handler : IRequestHandler<CreatePaymentCommand>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /// <inheritdoc />
        public async Task<Unit> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            appDbContext.Entry(request.PaymentToSave).State = EntityState.Added;
            appDbContext.Entry(request.PaymentToSave.ChargedAccount).State = EntityState.Modified;
            if (request.PaymentToSave.TargetAccount != null)
            {
                appDbContext.Entry(request.PaymentToSave.TargetAccount).State = EntityState.Modified;
            }

            if (request.PaymentToSave.IsRecurring)
            {
                if (request.PaymentToSave.RecurringPayment == null)
                {
                    RecurringPaymentNullException exception = new(
                        $"Recurring Payment for Payment {request.PaymentToSave.Id} is null, although payment is marked recurring.");

                    Log.Error(exception: exception, messageTemplate: "Error during Payment Creation");

                    throw exception;
                }

                appDbContext.Entry(request.PaymentToSave.RecurringPayment).State = EntityState.Added;
            }

            _ = await appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
