namespace MoneyFox.Core.Commands.Accounts.DeleteAccountById
{
    using Aggregates;
    using Common.Interfaces;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeactivateAccountByIdCommand : IRequest
    {
        public DeactivateAccountByIdCommand(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId { get; }

        public class Handler : IRequestHandler<DeactivateAccountByIdCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(DeactivateAccountByIdCommand request, CancellationToken cancellationToken)
            {
                Account entityToDeactivate = await contextAdapter.Context.Accounts.FindAsync(request.AccountId);
                entityToDeactivate.Deactivate();
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}