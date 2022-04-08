namespace MoneyFox.Core.Queries
{

    using System.Threading;
    using System.Threading.Tasks;
    using _Pending_.Exceptions;
    using Aggregates.Payments;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using NLog;

    public class GetPaymentByIdQuery : IRequest<Payment>
    {
        public GetPaymentByIdQuery(int paymentId)
        {
            PaymentId = paymentId;
        }

        public int PaymentId { get; }

        public class Handler : IRequestHandler<GetPaymentByIdQuery, Payment>
        {
            private readonly ILogger logger = LogManager.GetCurrentClassLogger();

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
                    .SingleOrDefaultAsync(x => x.Id == request.PaymentId);

                if (payment == null)
                {
                    logger.Error(message: "Payment with id {paymentId} not found.", argument: request.PaymentId);

                    throw new PaymentNotFoundException();
                }

                return payment;
            }
        }
    }

}
