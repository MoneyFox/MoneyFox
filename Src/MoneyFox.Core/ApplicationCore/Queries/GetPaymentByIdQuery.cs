namespace MoneyFox.Core.ApplicationCore.Queries
{

    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates.AccountAggregate;
    using Domain.Exceptions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using MoneyFox.Core.Common.Interfaces;

    public class GetPaymentByIdQuery : IRequest<Payment>
    {
        public GetPaymentByIdQuery(int paymentId)
        {
            PaymentId = paymentId;
        }

        private int PaymentId { get; }

        public class Handler : IRequestHandler<GetPaymentByIdQuery, Payment>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Payment> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
            {
                var payment = await contextAdapter.Context.Payments.Include(x => x.ChargedAccount)
                    .Include(x => x.TargetAccount)
                    .Include(x => x.RecurringPayment)
                    .Include(x => x.Category)
                    .SingleOrDefaultAsync(predicate: x => x.Id == request.PaymentId, cancellationToken: cancellationToken);

                if (payment == null)
                {
                    throw new PaymentNotFoundException();
                }

                return payment;
            }
        }
    }

}
