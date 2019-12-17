using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.QueryObjects;

namespace MoneyFox.Application.Accounts.Queries.GetIfAccountWithNameExists
{
    public class GetIfAccountWithNameExistsQuery : IRequest<bool>
    {
        public string AccountName { get; set; }

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
                return await contextAdapter.Context.Accounts.AnyWithNameAsync(request.AccountName);
            }
        }
    }
}
