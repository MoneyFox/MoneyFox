using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Categories.Command.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest
    {
        public Category Category { get; set; }

        public class Handler : IRequestHandler<UpdateCategoryCommand>
        {
            private readonly IContextAdapter contextAdapter;
            private readonly IBackupService backupService;

            public Handler(IContextAdapter contextAdapter, IBackupService backupService)
            {
                this.contextAdapter = contextAdapter;
                this.backupService = backupService;
            }

            public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                Category existingCategory = await contextAdapter.Context.Categories.FindAsync(request.Category.Id);

                existingCategory.UpdateData(request.Category.Name,
                                            request.Category.Note);

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);
                await backupService.UploadBackupAsync();

                return Unit.Value;
            }
        }
    }
}
