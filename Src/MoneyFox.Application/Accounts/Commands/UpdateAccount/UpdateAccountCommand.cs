using MediatR;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommand : IRequest
    {
        public Account Account { get; set; }

        public class Handler : IRequestHandler<UpdateAccountCommand>
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

            public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
            {
                Account existingAccount = await contextAdapter.Context
                                                              .Accounts
                                                              .FindAsync(request.Account.Id);

                existingAccount.UpdateAccount(request.Account.Name,
                                              request.Account.CurrentBalance,
                                              request.Account.Note,
                                              request.Account.IsExcluded);

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
                await backupService.UploadBackupAsync();

                return Unit.Value;
            }
        }
    }
}
