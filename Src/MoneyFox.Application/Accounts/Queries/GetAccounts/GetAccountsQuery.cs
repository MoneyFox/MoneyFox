using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Accounts.Queries.GetAccounts
{
    public class GetAccountsQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetAccountsQuery, List<Account>>
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<List<Account>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
            {
                return await context.Accounts.ToListAsync(cancellationToken);
            }
        }
    }
}
