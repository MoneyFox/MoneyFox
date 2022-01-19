using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_.Common.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Queries.Accounts.GetAccountNameById
{
    public class GetAccountNameByIdQuery : IRequest<string>
    {
        public GetAccountNameByIdQuery(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId { get; }

        public class Handler : IRequestHandler<GetAccountNameByIdQuery, string>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<string> Handle(GetAccountNameByIdQuery request, CancellationToken cancellationToken)
            {
                string? account = await contextAdapter.Context
                    .Accounts
                    .Where(x => x.Id == request.AccountId)
                    .Select(x => x.Name)
                    .FirstOrDefaultAsync(cancellationToken);

                return account ?? string.Empty;
            }
        }
    }
}