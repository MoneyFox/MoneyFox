namespace MoneyFox.Core.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetPaymentByIdQuery : IRequest<Payment>
{
    public GetPaymentByIdQuery(int paymentId)
    {
        PaymentId = paymentId;
    }

    private int PaymentId { get; }

    public class Handler : IRequestHandler<GetPaymentByIdQuery, Payment>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Payment> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = await appDbContext.Payments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .Include(x => x.RecurringPayment)
                .Include(x => x.Category)
                .SingleOrDefaultAsync(predicate: x => x.Id == request.PaymentId, cancellationToken: cancellationToken);

            return payment ?? throw new PaymentNotFoundException();
        }
    }
}
