namespace MoneyFox.Core.Features.CategoryDeletion;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class DeleteCategoryById
{
    public record Command(int CategoryId) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext dbContext;

        public Handler(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var paymentsWithCategory = await dbContext.Payments.Include(p => p.Category)
                .Where(p => p.Category != null)
                .Where(p => p.Category!.Id == command.CategoryId)
                .ToListAsync(cancellationToken);

            paymentsWithCategory.ForEach(p => p.RemoveCategory());
            var entityToDelete = await dbContext.Categories.FindAsync(command.CategoryId);
            if (entityToDelete is null)
            {
                return Unit.Value;
            }

            dbContext.Categories.Remove(entityToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
