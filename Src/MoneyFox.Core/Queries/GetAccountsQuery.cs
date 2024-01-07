namespace MoneyFox.Core.Queries;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Common.Settings;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAccountsQuery : IRequest<IReadOnlyList<GetAccountsQuery.AccountData>>
{
    public class Handler(IAppDbContext appDbContext, ISettingsFacade settingsFacade) : IRequestHandler<GetAccountsQuery, IReadOnlyList<AccountData>>
    {
        public async Task<IReadOnlyList<AccountData>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var accounts = await appDbContext.Accounts.AreActive().OrderByInclusion().OrderByName().ToListAsync(cancellationToken);

            return accounts.Select(
                    a => new AccountData(
                        Id: a.Id,
                        Name: a.Name,
                        CurrentBalance: new(amount: a.CurrentBalance, currencyAlphaIsoCode: settingsFacade.DefaultCurrency),
                        IsExcluded: a.IsExcluded))
                .ToImmutableList();
        }
    }

    public record AccountData(int Id, string Name, Money CurrentBalance, bool IsExcluded);
}
