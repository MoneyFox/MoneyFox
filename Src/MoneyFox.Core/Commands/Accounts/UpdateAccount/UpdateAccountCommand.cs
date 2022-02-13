using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Commands.Accounts.UpdateAccount
{
    public class UpdateAccountCommand : IRequest
    {
        public UpdateAccountCommand(Account account)
        {
            Account = account;
        }

        public Account Account { get; }

        public class Handler : IRequestHandler<UpdateAccountCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
            {
                Account existingAccount = await contextAdapter.Context
                    .Accounts
                    .FindAsync(request.Account.Id);

                existingAccount.UpdateAccount(
                    name: request.Account.Name,
                    note: request.Account.Note ?? "",
                    isExcluded: request.Account.IsExcluded);

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}