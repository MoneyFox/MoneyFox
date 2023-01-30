namespace MoneyFox.Core.Queries;

using System.Threading;
using System.Threading.Tasks;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using MediatR;

public class GetIfAccountWithNameExistsQuery : IRequest<bool>
{
    public GetIfAccountWithNameExistsQuery(string accountName, int accountId)
    {
        AccountName = accountName;
        AccountId = accountId;
    }

    public string AccountName { get; }

    public int AccountId { get; }

    public class Handler : IRequestHandler<GetIfAccountWithNameExistsQuery, bool>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<bool> Handle(GetIfAccountWithNameExistsQuery request, CancellationToken cancellationToken)
        {
            return await appDbContext.Accounts.AnyWithNameAsync(name: request.AccountName, Id: request.AccountId);
        }
    }
}
