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
    public class Handler(IAppDbContext appDbContext) : IRequestHandler<GetAccountsQuery, IReadOnlyList<Account>>
    {
        public async Task<IReadOnlyList<Account>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var accounts = await appDbContext.Accounts.AreActive().OrderByInclusion().OrderByName().ToListAsync(cancellationToken);

            return accounts;
        }
    }
}
