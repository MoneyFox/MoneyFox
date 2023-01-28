namespace MoneyFox.Core.Commands.Accounts.DeleteAccountById;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class DeactivateAccountByIdCommand : IRequest
{
    public DeactivateAccountByIdCommand(int accountId)
    {
        AccountId = accountId;
    }

    public int AccountId { get; }

    public class Handler : IRequestHandler<DeactivateAccountByIdCommand>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(DeactivateAccountByIdCommand request, CancellationToken cancellationToken)
        {
            var entityToDeactivate = await appDbContext.Accounts.SingleAsync(predicate: a => a.Id == request.AccountId, cancellationToken: cancellationToken);
            entityToDeactivate.Deactivate();
            _ = await appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
