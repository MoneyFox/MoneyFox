namespace MoneyFox.Core.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAccountByIdQuery(int accountId) : IRequest<Account>
{
    public int AccountId { get; } = accountId;

    public class Handler(IAppDbContext dbContext) : IRequestHandler<GetAccountByIdQuery, Account>
    {
        public Task<Account> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            return dbContext.Accounts.FirstAsync(predicate: a => a.Id == request.AccountId, cancellationToken: cancellationToken);
        }
    }
}
