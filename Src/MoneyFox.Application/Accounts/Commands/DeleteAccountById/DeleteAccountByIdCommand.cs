using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Interfaces;

namespace MoneyFox.Application.Accounts.Commands.DeleteAccountById
{
    public class DeleteAccountByIdCommand : IRequest
    {
        public int AccountId { get; set; }

        public class Handler : IRequestHandler<DeleteAccountByIdCommand>
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(DeleteAccountByIdCommand request, CancellationToken cancellationToken)
            {
                var entityToDelete = await context.Accounts.FindAsync(request.AccountId, cancellationToken);

                context.Accounts.Remove(entityToDelete);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
