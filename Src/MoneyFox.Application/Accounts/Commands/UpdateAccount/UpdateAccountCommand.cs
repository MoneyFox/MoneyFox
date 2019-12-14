using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommand : IRequest
    {
        public Account Account { get; set; }

        public class Handler : IRequestHandler<UpdateAccountCommand>
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context)
            {
                this.context = context;
            }

            public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
            {
                Account existingAccount = await context.Accounts.FindAsync(request.Account.Id);

                existingAccount.UpdateAccount(request.Account.Name,
                                              request.Account.CurrentBalance,
                                              request.Account.Note,
                                              request.Account.IsExcluded);

                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
