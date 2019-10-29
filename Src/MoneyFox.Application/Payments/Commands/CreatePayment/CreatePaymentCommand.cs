using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
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
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            /// <inheritdoc />
            public async Task<Unit> Handle(CreatePaymentCommand request, CancellationToken cancellationToken) 
            {
                context.Entry(request.PaymentToSave).State = EntityState.Added;

                if (request.PaymentToSave.IsRecurring) {
                    context.Entry(request.PaymentToSave.RecurringPayment).State = EntityState.Added;
                }
                
                await context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
