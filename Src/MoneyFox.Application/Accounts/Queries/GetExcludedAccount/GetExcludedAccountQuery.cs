using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Accounts.Queries.GetExcludedAccount
{
    public class GetExcludedAccountQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetExcludedAccountQuery, List<Account>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<List<Account>> Handle(GetExcludedAccountQuery request, CancellationToken cancellationToken)
            {
                return await contextAdapter.Context
                                           .Accounts
                                           .AreExcluded()
                                           .OrderByName()
                                           .ToListAsync(cancellationToken);
            }
        }
    }
}
