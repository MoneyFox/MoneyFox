using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQuery : IRequest<Account>
    {
        public GetAccountByIdQuery(int accountId) {
            AccountId = accountId;
        }

        public int AccountId { get; }

        public class Handler : IRequestHandler<GetAccountByIdQuery, Account> 
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context) {
                this.context = context;
            }

            public async Task<Account> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken) 
            {
                return await context.Accounts.FindAsync(request.AccountId);
            }
        }
    }
}
