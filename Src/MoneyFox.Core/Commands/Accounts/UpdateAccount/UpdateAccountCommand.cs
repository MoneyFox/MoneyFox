namespace MoneyFox.Core.Commands.Accounts.UpdateAccount
{

    using System.Threading;
    using System.Threading.Tasks;
    using ApplicationCore.Domain.Aggregates.AccountAggregate;
    using Common.Interfaces;
    using MediatR;

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
                var existingAccount = await appDbContext.Accounts.FindAsync(request.Account.Id);
                existingAccount.Change(name: request.Account.Name, note: request.Account.Note ?? "", isExcluded: request.Account.IsExcluded);
                await appDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }

}
