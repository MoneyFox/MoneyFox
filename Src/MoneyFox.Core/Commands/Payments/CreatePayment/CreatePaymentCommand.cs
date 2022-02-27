namespace MoneyFox.Core.Commands.Payments.CreatePayment
{
    using _Pending_.Common.Interfaces;
    using _Pending_.Exceptions;
    using Aggregates.Payments;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using NLog;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreatePaymentCommand : IRequest
    {
        public CreatePaymentCommand(Payment paymentToSave)
        {
            PaymentToSave = paymentToSave;
        }

        public Payment PaymentToSave { get; }

        public class Handler : IRequestHandler<CreatePaymentCommand>
        {
            private readonly Logger logger = LogManager.GetCurrentClassLogger();

            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            /// <inheritdoc />
            public async Task<Unit> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
            {
                contextAdapter.Context.Entry(request.PaymentToSave).State = EntityState.Added;
                contextAdapter.Context.Entry(request.PaymentToSave.ChargedAccount).State = EntityState.Modified;

                if(request.PaymentToSave.TargetAccount != null)
                {
                    contextAdapter.Context.Entry(request.PaymentToSave.TargetAccount).State = EntityState.Modified;
                }

                if(request.PaymentToSave.IsRecurring)
                {
                    if(request.PaymentToSave.RecurringPayment == null)
                    {
                        var exception = new RecurringPaymentNullException(
                            $"Recurring Payment for Payment {request.PaymentToSave.Id} is null, although payment is marked recurring.");
                        logger.Error(exception);
                        throw exception;
                    }

                    contextAdapter.Context.Entry(request.PaymentToSave.RecurringPayment).State = EntityState.Added;
                }

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}