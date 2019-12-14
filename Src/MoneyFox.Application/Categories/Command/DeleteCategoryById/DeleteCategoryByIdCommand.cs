using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Categories.Command.DeleteCategoryById
{
    public class DeleteCategoryByIdCommand : IRequest
    {
        public DeleteCategoryByIdCommand(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; }

        public class Handler : IRequestHandler<DeleteCategoryByIdCommand>
        {
            private readonly IEfCoreContext context;
            private readonly IBackupService backupService;

            public Handler(IEfCoreContext context, IBackupService backupService)
            {
                this.context = context;
                this.backupService = backupService;
            }

            public async Task<Unit> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
            {
                await backupService.RestoreBackupAsync();
                Category entityToDelete = await context.Categories.FindAsync(request.CategoryId);

                context.Categories.Remove(entityToDelete);
                await context.SaveChangesAsync(cancellationToken);

                await backupService.UploadBackupAsync();

                return Unit.Value;
            }
        }
    }
}
