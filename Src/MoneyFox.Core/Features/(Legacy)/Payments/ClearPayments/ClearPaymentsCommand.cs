namespace MoneyFox.Core.Features._Legacy_.Payments.ClearPayments;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class ClearPaymentsCommand : IRequest
{
    public class Handler : IRequestHandler<ClearPaymentsCommand>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task Handle(ClearPaymentsCommand request, CancellationToken cancellationToken)
        {
            var unclearedPayments = await appDbContext.Payments.Include(x => x.ChargedAccount)
                .Include(x => x.TargetAccount)
                .AsQueryable()
                .AreNotCleared()
                .ToListAsync(cancellationToken: cancellationToken);

            foreach (var payment in unclearedPayments)
            {
                payment.ClearPayment();
            }

            _ = await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
