namespace MoneyFox.Core.Commands.Accounts.DeleteAccountById
{

    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using MediatR;

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
                var entityToDeactivate = await contextAdapter.Context.Accounts.FindAsync(request.AccountId);
                entityToDeactivate?.Deactivate();
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }

}
