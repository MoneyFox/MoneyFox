using MediatR;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.DbBackup;
using MoneyFox.Core.Aggregates.Payments;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Application.Categories.Command.CreateCategory
{
    public class CreateCategoryCommand : IRequest
    {
        public CreateCategoryCommand(string name, string note = "", bool requireNote = false)
        {
            Name = name;
            Note = note;
            RequireNote = requireNote;
        }

        public string Name { get; }
        public string Note { get; }
        public bool RequireNote { get; }

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

            /// <inheritdoc />
            public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                await backupService.RestoreBackupAsync();

                await contextAdapter.Context.Categories.AddAsync(
                    new Category(request.Name, request.Note, request.RequireNote),
                    cancellationToken);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                settingsFacade.LastDatabaseUpdate = DateTime.Now;
                backupService.UploadBackupAsync().FireAndForgetSafeAsync();

                return Unit.Value;
            }
        }
    }
}