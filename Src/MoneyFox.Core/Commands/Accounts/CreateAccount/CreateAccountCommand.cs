namespace MoneyFox.Core.Commands.Accounts.CreateAccount
{

    using System.Threading;
    using System.Threading.Tasks;
    using ApplicationCore.Domain.Aggregates.AccountAggregate;
    using Common.Interfaces;
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

        public string Name { get; }
        public decimal CurrentBalance { get; }
        public string Note { get; }
        public bool IsExcluded { get; }

        public class Handler : IRequestHandler<CreateAccountCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            /// <inheritdoc />
            public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
            {
                var account = new Account(name: request.Name, initalBalance: request.CurrentBalance, note: request.Note, isExcluded: request.IsExcluded);
                await contextAdapter.Context.Accounts.AddAsync(entity: account, cancellationToken: cancellationToken);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }

}
