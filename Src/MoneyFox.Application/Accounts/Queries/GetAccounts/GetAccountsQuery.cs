using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Accounts.Queries.GetAccounts
{
    public class GetAccountsQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetAccountsQuery, List<Account>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<List<Account>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
            {
                List<Account>? accounts = await contextAdapter.Context
                                                   .Accounts
                                                   .AreActive()
                                                   .OrderByInclusion()
                                                   .OrderByName()
                                                   .ToListAsync(cancellationToken);

                return accounts;
            }
        }
    }
}
