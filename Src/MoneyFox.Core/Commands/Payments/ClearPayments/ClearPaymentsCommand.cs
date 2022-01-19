using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Common.QueryObjects;
using MoneyFox.Core.Aggregates.Payments;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Commands.Payments.ClearPayments
{
    public class ClearPaymentsCommand : IRequest
    {
        public class Handler : IRequestHandler<ClearPaymentsCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(ClearPaymentsCommand request, CancellationToken cancellationToken)
            {
                List<Payment> unclearedPayments = await contextAdapter.Context
                    .Payments
                    .Include(x => x.ChargedAccount)
                    .Include(x => x.TargetAccount)
                    .AsQueryable()
                    .AreNotCleared()
                    .ToListAsync();

                foreach(Payment payment in unclearedPayments)
                {
                    payment.ClearPayment();
                }

                await contextAdapter.Context.SaveChangesAsync();
                return Unit.Value;
            }
        }
    }
}