using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Common.QueryObjects;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Queries.Accounts.GetIfAccountWithNameExists
{
    public class GetIfAccountWithNameExistsQuery : IRequest<bool>
    {
        public GetIfAccountWithNameExistsQuery(string accountName)
        {
            AccountName = accountName;
        }

        public string AccountName { get; }

        public class Handler : IRequestHandler<GetIfAccountWithNameExistsQuery, bool>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            /// <inheritdoc />
            public async Task<bool> Handle(GetIfAccountWithNameExistsQuery request, CancellationToken cancellationToken)
                => await contextAdapter.Context.Accounts.AnyWithNameAsync(request.AccountName);
        }
    }
}