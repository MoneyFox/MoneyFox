using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Interfaces;
using MoneyFox.Application.QueryObjects;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Accounts.Queries.GetIncludedAccount
{
    public class GetIncludedAccountQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetIncludedAccountQuery, List<Account>>
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<List<Account>> Handle(GetIncludedAccountQuery request, CancellationToken cancellationToken)
            {
                return await context.Accounts
                                    .AreNotExcluded()
                                    .OrderByName()
                                    .ToListAsync(cancellationToken);
            }
        }
    }
}
