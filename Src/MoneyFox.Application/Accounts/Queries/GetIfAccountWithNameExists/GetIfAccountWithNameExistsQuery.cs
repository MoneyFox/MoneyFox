using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Interfaces;
using MoneyFox.Application.QueryObjects;

namespace MoneyFox.Application.Accounts.Queries.GetIfAccountWithNameExists
{
    public class GetIfAccountWithNameExistsQuery : IRequest<bool>
    {
        public string AccountName { get; set; }

        public class Handler : IRequestHandler<GetIfAccountWithNameExistsQuery, bool>
        {
            private IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            /// <inheritdoc />
            public async Task<bool> Handle(GetIfAccountWithNameExistsQuery request, CancellationToken cancellationToken)
            {
                return await context.Accounts.AnyWithNameAsync(request.AccountName);
            }
        }
    }
}
