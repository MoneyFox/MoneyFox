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

                existingAccount.UpdateAccount(request.Account.Name,
                                              request.Account.CurrentBalance,
                                              request.Account.Note,
                                              request.Account.IsExcluded);

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
