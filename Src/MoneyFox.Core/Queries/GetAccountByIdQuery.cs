namespace MoneyFox.Core.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAccountByIdQuery : IRequest<Account>
{
    public GetAccountByIdQuery(int accountId)
    {
        AccountId = accountId;
    }

    public int AccountId { get; }

    public class Handler : IRequestHandler<GetAccountByIdQuery, Account>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Account> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Accounts.FirstAsync(predicate: a => a.Id == request.AccountId, cancellationToken: cancellationToken);
        }
    }
}
