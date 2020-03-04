using MediatR;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Accounts.Commands.DeleteAccountById
{
    public class DeleteAccountByIdCommand : IRequest
    {
        public DeleteAccountByIdCommand(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId { get; }

        public class Handler : IRequestHandler<DeleteAccountByIdCommand>
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

            public async Task<Unit> Handle(DeleteAccountByIdCommand request, CancellationToken cancellationToken)
            {
                Account entityToDelete = await contextAdapter.Context.Accounts.FindAsync(request.AccountId);

                contextAdapter.Context.Accounts.Remove(entityToDelete);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                settingsFacade.LastDatabaseUpdate = DateTime.Now;
                await backupService.UploadBackupAsync();

                return Unit.Value;
            }
        }
    }
}
