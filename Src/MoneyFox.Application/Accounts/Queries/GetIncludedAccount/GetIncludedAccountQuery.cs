using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;
using MoneyFox.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Accounts.Queries.GetIncludedAccount
{
    public class GetIncludedAccountQuery : IRequest<List<Account>>
    {
        public class Handler : IRequestHandler<GetIncludedAccountQuery, List<Account>>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<List<Account>> Handle(GetIncludedAccountQuery request, CancellationToken cancellationToken)
            {
                return await contextAdapter.Context
                                           .Accounts
                                           .AreActive()
                                           .AreNotExcluded()
                                           .OrderByName()
                                           .ToListAsync(cancellationToken);
            }
        }
    }
}
