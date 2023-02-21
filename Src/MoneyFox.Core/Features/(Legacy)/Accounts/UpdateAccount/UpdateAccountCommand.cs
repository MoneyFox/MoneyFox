namespace MoneyFox.Core.Features._Legacy_.Accounts.UpdateAccount;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class UpdateAccount
{
    public record Command(int Id, string Name, string? Note, bool IsExcluded) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var existingAccount = await appDbContext.Accounts.SingleAsync(predicate: b => b.Id == command.Id, cancellationToken: cancellationToken);
            existingAccount.Change(name: command.Name, note: command.Note ?? "", isExcluded: command.IsExcluded);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
