using MediatR;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Commands.Accounts.CreateAccount
{
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
            private readonly IPublisher publisher;
            private readonly ISettingsFacade settingsFacade;

            public Handler(
                IContextAdapter contextAdapter,
                IPublisher publisher,
                ISettingsFacade settingsFacade)
            {
                this.contextAdapter = contextAdapter;
                this.publisher = publisher;
                this.settingsFacade = settingsFacade;
            }

            /// <inheritdoc />
            public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
            {
                var account = new Account(
                    request.Name,
                    request.CurrentBalance,
                    request.Note,
                    request.IsExcluded);

                await contextAdapter.Context.Accounts.AddAsync(
                    account,
                    cancellationToken);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                // TODO: move to ef core context
                settingsFacade.LastDatabaseUpdate = DateTime.Now;
                await publisher.Publish(new AccountCreatedEvent(account.Id), cancellationToken);
                return Unit.Value;
            }
        }
    }
}