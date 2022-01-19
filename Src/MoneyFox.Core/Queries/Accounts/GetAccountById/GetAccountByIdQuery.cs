using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Queries.Accounts.GetAccountById
{
    public class GetAccountByIdQuery : IRequest<Account>
    {
        public GetAccountByIdQuery(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId { get; }

        public class Handler : IRequestHandler<GetAccountByIdQuery, Account>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Account> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken) =>
                await contextAdapter.Context.Accounts.FindAsync(request.AccountId);
        }
    }
}