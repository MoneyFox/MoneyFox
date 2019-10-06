using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommand : IRequest
    {
        public UpdatePaymentCommand(Payment payment) {
            Payment = payment;
        }

        public Payment Payment { get; }

        public class Handler : IRequestHandler<UpdatePaymentCommand>
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
            {
                var existingPayment = await context.Payments.FindAsync(request.Payment.Id);

                existingPayment.UpdatePayment(request.Payment.Date,
                                              request.Payment.Amount,
                                              request.Payment.Type,
                                              request.Payment.ChargedAccount,
                                              request.Payment.TargetAccount,
                                              request.Payment.Category,
                                              request.Payment.Note);

                await context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
