namespace MoneyFox.Core.Features._Legacy_.Accounts.CreateAccount;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.AccountAggregate;
using MediatR;

public class CreateAccountCommand : IRequest
{
    public CreateAccountCommand(string name, decimal currentBalance = 0, string note = "", bool isExcluded = false)
    {
        Name = name;
        CurrentBalance = currentBalance;
        Note = note;
        IsExcluded = isExcluded;
    }

    private string Name { get; }
    private decimal CurrentBalance { get; }
    private string Note { get; }
    private bool IsExcluded { get; }

    public class Handler : IRequestHandler<CreateAccountCommand>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            Account account = new(name: request.Name, initialBalance: request.CurrentBalance, note: request.Note, isExcluded: request.IsExcluded);
            await appDbContext.Accounts.AddAsync(entity: account, cancellationToken: cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
