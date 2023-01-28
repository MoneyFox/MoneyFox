namespace MoneyFox.Core.Commands.Accounts.UpdateAccount;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateAccountCommand : IRequest
{
    public UpdateAccountCommand(Account account)
    {
        Account = account;
    }

    public Account Account { get; }

    public class Handler : IRequestHandler<UpdateAccountCommand>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var existingAccount = await appDbContext.Accounts.SingleAsync(predicate: a => a.Id == request.Account.Id, cancellationToken: cancellationToken);
            existingAccount.Change(name: request.Account.Name, note: request.Account.Note ?? "", isExcluded: request.Account.IsExcluded);
            _ = await appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
