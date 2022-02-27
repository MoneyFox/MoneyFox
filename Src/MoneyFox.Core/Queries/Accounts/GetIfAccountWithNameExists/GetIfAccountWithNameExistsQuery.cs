namespace MoneyFox.Core.Queries.Accounts.GetIfAccountWithNameExists
{
    using _Pending_.Common.Interfaces;
    using _Pending_.Common.QueryObjects;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

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
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            /// <inheritdoc />
            public async Task<bool> Handle(GetIfAccountWithNameExistsQuery request, CancellationToken cancellationToken)
                => await contextAdapter.Context.Accounts.AnyWithNameAsync(request.AccountName, request.AccountId);
        }
    }
}