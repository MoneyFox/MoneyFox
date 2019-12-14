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
            private readonly IContextAdapter contextAdapter;
            private readonly IBackupService backupService;

            public Handler(IContextAdapter contextAdapter, IBackupService backupService)
            {
                this.contextAdapter = contextAdapter;
                this.backupService = backupService;
            }

            public async Task<Unit> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
            {
                await backupService.RestoreBackupAsync();
                Category entityToDelete = await contextAdapter.Context.Categories.FindAsync(request.CategoryId);

                contextAdapter.Context.Categories.Remove(entityToDelete);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                await backupService.UploadBackupAsync();

                return Unit.Value;
            }
        }
    }
}
