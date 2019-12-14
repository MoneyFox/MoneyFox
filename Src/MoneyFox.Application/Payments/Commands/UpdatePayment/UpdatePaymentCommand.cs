using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommand : IRequest
    {
        public UpdatePaymentCommand(Payment payment)
        {
            Payment = payment;
        }

        public Payment Payment { get; }

        public class Handler : IRequestHandler<UpdatePaymentCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
            {
                Payment existingPayment = await contextAdapter.Context.Payments
                                                                      .FindAsync(request.Payment.Id);

                if(existingPayment != null)
                {
                    contextAdapter.Context.Entry(existingPayment).CurrentValues.SetValues(request.Payment);
                }

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
