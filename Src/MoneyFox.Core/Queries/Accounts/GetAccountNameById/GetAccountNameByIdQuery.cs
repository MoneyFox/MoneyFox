namespace MoneyFox.Core.Queries.Accounts.GetAccountNameById
{

    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

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
                var account = await contextAdapter.Context.Accounts.Where(x => x.Id == request.AccountId)
                    .Select(x => x.Name)
                    .FirstOrDefaultAsync(cancellationToken);

                return account ?? string.Empty;
            }
        }
    }

}
