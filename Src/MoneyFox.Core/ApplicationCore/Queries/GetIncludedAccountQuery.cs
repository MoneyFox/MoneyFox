namespace MoneyFox.Core.ApplicationCore.Queries;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _Pending_.Common.QueryObjects;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetIncludedAccountQuery : IRequest<List<Account>>
{
    public class Handler : IRequestHandler<GetIncludedAccountQuery, List<Account>>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<Account>> Handle(GetIncludedAccountQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Accounts.AreActive().AreNotExcluded().OrderByName().ToListAsync(cancellationToken);
        }
    }
}
