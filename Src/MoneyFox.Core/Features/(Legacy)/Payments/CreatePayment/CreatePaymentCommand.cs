namespace MoneyFox.Core.Features._Legacy_.Payments.CreatePayment;

using System.Threading;
using System.Threading.Tasks;
using Aptabase.Maui;
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
        private readonly IAptabaseClient aptabaseClient;

        public Handler(IAppDbContext appDbContext, IAptabaseClient aptabaseClient)
        {
            this.appDbContext = appDbContext;
            this.aptabaseClient = aptabaseClient;
        }

        /// <inheritdoc />
        public async Task Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
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

            await appDbContext.SaveChangesAsync(cancellationToken);
            aptabaseClient.TrackEvent("payment_created");
        }
    }
}
