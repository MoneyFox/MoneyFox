namespace MoneyFox.Core.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Common.Settings;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetAccountById
{
    public record Query(int AccountId) : IRequest<AccountData>;

    public class Handler(IAppDbContext dbContext, ISettingsFacade settingsFacade) : IRequestHandler<Query, AccountData>
    {
        public async Task<AccountData> Handle(Query query, CancellationToken cancellationToken)
        {
            var account = await dbContext.Accounts.FirstAsync(predicate: a => a.Id == query.AccountId, cancellationToken: cancellationToken);

            return new(
                AccountId: account.Id,
                Name: account.Name,
                CurrentBalance: new(amount: account.CurrentBalance, currencyAlphaIsoCode: settingsFacade.DefaultCurrency),
                IsExcluded: account.IsExcluded);
        }
    }

    public record AccountData(int AccountId, string Name, Money CurrentBalance, bool IsExcluded);
}
