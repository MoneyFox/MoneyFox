using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Interfaces;

namespace MoneyFox.Application.Payments.Commands.DeletePaymentById
{
    public class DeletePaymentByIdCommand : IRequest
    {
        public DeletePaymentByIdCommand(int paymentId) {
            PaymentId = paymentId;
        }

        public int PaymentId { get; }

        public class Handler : IRequestHandler<DeletePaymentByIdCommand> {
            
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context) {
                this.context = context;
            }

            public async Task<Unit> Handle(DeletePaymentByIdCommand request, CancellationToken cancellationToken) {
                var entityToDelete = await context.Payments.FindAsync(request.PaymentId);

                context.Payments.Remove(entityToDelete);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;

            }
        }
    }
}
