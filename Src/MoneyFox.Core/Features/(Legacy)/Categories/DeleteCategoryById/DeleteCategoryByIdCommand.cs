namespace MoneyFox.Core.Features._Legacy_.Categories.DeleteCategoryById;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;

public class DeleteCategoryByIdCommand : IRequest
{
    public DeleteCategoryByIdCommand(int categoryId)
    {
        CategoryId = categoryId;
    }

    public int CategoryId { get; }

    public class Handler : IRequestHandler<DeleteCategoryByIdCommand>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
        {
            var entityToDelete = await appDbContext.Categories.FindAsync(request.CategoryId);
            if (entityToDelete is null)
            {
                return Unit.Value;
            }

            appDbContext.Categories.Remove(entityToDelete);
            await appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
