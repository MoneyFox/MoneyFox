namespace MoneyFox.Core.Commands.Accounts.UpdateAccount
{
    using _Pending_.Common.Interfaces;
    using Aggregates;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

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
                    request.Account.Name,
                    request.Account.Note ?? "",
                    request.Account.IsExcluded);

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}