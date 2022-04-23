namespace MoneyFox.Core.ApplicationCore.Queries
{

    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using MoneyFox.Core._Pending_.Common.QueryObjects;
    using MoneyFox.Core.Common.Interfaces;

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
            {
                return await contextAdapter.Context.Accounts.AnyWithNameAsync(name: request.AccountName, Id: request.AccountId);
            }
        }
    }

}
