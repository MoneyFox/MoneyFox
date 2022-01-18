using MediatR;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.DbBackup;
using MoneyFox.Core.Aggregates;
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
            private readonly IBackupService backupService;
            private readonly ISettingsFacade settingsFacade;

            public Handler(IContextAdapter contextAdapter,
                IBackupService backupService,
                ISettingsFacade settingsFacade)
            {
                this.contextAdapter = contextAdapter;
                this.backupService = backupService;
                this.settingsFacade = settingsFacade;
            }

            /// <inheritdoc />
            public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
            {
                await contextAdapter.Context.Accounts.AddAsync(
                    new Account(
                        request.Name,
                        request.CurrentBalance,
                        request.Note,
                        request.IsExcluded),
                    cancellationToken);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                settingsFacade.LastDatabaseUpdate = DateTime.Now;
                
                // TODO: Use notification
                //backupService.UploadBackupAsync().FireAndForgetSafeAsync();

                return Unit.Value;
            }
        }
    }
}