namespace MoneyFox.Core.Features._Legacy_.Categories.UpdateCategory;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class UpdateCategory
{
    public record Command(int Id, string Name, string? Note, bool RequireNote) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var existingCategory = await appDbContext.Categories.SingleAsync(predicate: b => b.Id == command.Id, cancellationToken: cancellationToken);
            existingCategory.UpdateData(name: command.Name, note: command.Note ?? "", requireNote: command.RequireNote);
            await appDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
