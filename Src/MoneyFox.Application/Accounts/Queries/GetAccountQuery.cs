using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Accounts.Queries
{
    public class GetAccountQuery : IRequest<List<Account>>
    {
    }

    public class Handler : IRequestHandler<GetAccountQuery, List<Account>>
    {
        private readonly IEfCoreContext context;

        public Handler(IEfCoreContext context)
        {
            this.context = context;
        }
        
        public async Task<List<Account>> Handle(GetAccountQuery request, CancellationToken cancellationToken) {
            return await context.Accounts.ToListAsync(cancellationToken);
        }
    }
}
