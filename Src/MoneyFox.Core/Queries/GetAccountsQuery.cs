namespace MoneyFox.Core.Queries;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAccountsQuery : IRequest<IReadOnlyList<Account>>
{
    public class Handler : IRequestHandler<GetAccountsQuery, IReadOnlyList<Account>>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IReadOnlyList<Account>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var accounts = await appDbContext.Accounts.AreActive().OrderByInclusion().OrderByName().ToListAsync(cancellationToken);

            return accounts;
        }
    }
}
