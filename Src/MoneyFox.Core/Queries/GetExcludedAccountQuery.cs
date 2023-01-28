namespace MoneyFox.Core.Queries;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetExcludedAccountQuery : IRequest<List<Account>>
{
    public class Handler : IRequestHandler<GetExcludedAccountQuery, List<Account>>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<Account>> Handle(GetExcludedAccountQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Accounts.AreActive().AreExcluded().OrderByName().ToListAsync(cancellationToken);
        }
    }
}
