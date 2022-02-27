namespace MoneyFox.Core.Queries.Accounts.GetAccountById
{
    using _Pending_.Common.Interfaces;
    using Aggregates;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

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