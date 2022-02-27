namespace MoneyFox.Core.Commands.Accounts.CreateAccount
{
    using _Pending_.Common.Interfaces;
    using Aggregates;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

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
                var account = new Account(
                    request.Name,
                    request.CurrentBalance,
                    request.Note,
                    request.IsExcluded);

                await contextAdapter.Context.Accounts.AddAsync(account, cancellationToken);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}