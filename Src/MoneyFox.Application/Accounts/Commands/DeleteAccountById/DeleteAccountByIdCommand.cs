using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Accounts.Commands.DeleteAccountById
{
    public class DeleteAccountByIdCommand : IRequest
    {
        public DeleteAccountByIdCommand(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId { get; }

        public class Handler : IRequestHandler<DeleteAccountByIdCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(DeleteAccountByIdCommand request, CancellationToken cancellationToken)
            {
                Account entityToDelete = await contextAdapter.Context.Accounts.FindAsync(request.AccountId);

                contextAdapter.Context.Accounts.Remove(entityToDelete);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
