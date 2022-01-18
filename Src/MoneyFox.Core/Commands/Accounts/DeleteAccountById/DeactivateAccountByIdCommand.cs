using MediatR;
using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.DbBackup;
using MoneyFox.Core.Aggregates;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Commands.Accounts.DeleteAccountById
{
    public class DeactivateAccountByIdCommand : IRequest
    {
        public DeactivateAccountByIdCommand(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId { get; }

        public class Handler : IRequestHandler<DeactivateAccountByIdCommand>
        {
            private readonly IContextAdapter contextAdapter;
            private readonly IBackupService backupService;
            private readonly ISettingsFacade settingsFacade;

            public Handler(IContextAdapter contextAdapter, IBackupService backupService, ISettingsFacade settingsFacade)
            {
                this.contextAdapter = contextAdapter;
                this.backupService = backupService;
                this.settingsFacade = settingsFacade;
            }

            public async Task<Unit> Handle(DeactivateAccountByIdCommand request, CancellationToken cancellationToken)
            {
                Account entityToDeactivate = await contextAdapter.Context.Accounts.FindAsync(request.AccountId);
                entityToDeactivate.Deactivate();
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                settingsFacade.LastDatabaseUpdate = DateTime.Now;
                backupService.UploadBackupAsync().FireAndForgetSafeAsync();

                return Unit.Value;
            }
        }
    }
}