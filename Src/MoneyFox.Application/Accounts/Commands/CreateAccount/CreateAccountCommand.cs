using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest
    {
        public Account AccountToSave { get; set; }

        public class Handler : IRequestHandler<CreateAccountCommand>
        {
            private IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            /// <inheritdoc />
            public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
            {
                await context.Accounts.AddAsync(request.AccountToSave, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
