using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Payments.Queries.GetPaymentById
{
    public class GetPaymentByIdQuery : IRequest<Payment>
    {
        public GetPaymentByIdQuery(int paymentId)
        {
            PaymentId = paymentId;
        }

        public int PaymentId { get; }

        public class Handler : IRequestHandler<GetPaymentByIdQuery, Payment>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Payment> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
            {
                return await contextAdapter.Context.Payments.FindAsync(request.PaymentId);
            }
        }
    }
}
