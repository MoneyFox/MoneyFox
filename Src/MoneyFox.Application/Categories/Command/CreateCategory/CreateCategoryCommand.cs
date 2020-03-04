using MediatR;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Categories.Command.CreateCategory
{
    public class CreateCategoryCommand : IRequest
    {
        public CreateCategoryCommand(Category categoryToSave)
        {
            CategoryToSave = categoryToSave;
        }

        public Category CategoryToSave { get; }

        public class Handler : IRequestHandler<CreateCategoryCommand>
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

            /// <inheritdoc/>
            public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                await backupService.RestoreBackupAsync();

                await contextAdapter.Context.Categories.AddAsync(request.CategoryToSave, cancellationToken);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                settingsFacade.LastDatabaseUpdate = DateTime.Now;
                await backupService.UploadBackupAsync();

                return Unit.Value;
            }
        }
    }
}
