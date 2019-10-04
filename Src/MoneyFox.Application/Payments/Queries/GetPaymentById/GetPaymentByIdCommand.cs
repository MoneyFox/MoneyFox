using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Payments.Queries.GetPaymentById
{
    public class GetPaymentByIdCommand : IRequest<Payment>
    {
        public GetPaymentByIdCommand(int paymentId)
        {
            PaymentId = paymentId;
        }

        public int PaymentId { get; }

        public class Handler : IRequestHandler<GetPaymentByIdCommand, Payment>
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<Payment> Handle(GetPaymentByIdCommand request, CancellationToken cancellationToken)
            {
                return await context.Payments.FindAsync(request.PaymentId);
            }
        }
    }
}
