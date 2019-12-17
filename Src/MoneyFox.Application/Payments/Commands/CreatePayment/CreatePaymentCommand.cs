using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest
    {
        public CreatePaymentCommand(Payment paymentToSave)
        {
            PaymentToSave = paymentToSave;
        }

        public Payment PaymentToSave { get; }

        public class Handler : IRequestHandler<CreatePaymentCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            /// <inheritdoc />
            public async Task<Unit> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
            {
                contextAdapter.Context.Entry(request.PaymentToSave).State = EntityState.Added;

                if (request.PaymentToSave.IsRecurring)
                {
                    contextAdapter.Context.Entry(request.PaymentToSave.RecurringPayment).State = EntityState.Added;
                }

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}