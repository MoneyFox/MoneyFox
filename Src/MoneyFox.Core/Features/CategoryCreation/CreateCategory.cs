namespace MoneyFox.Core.Features.CategoryCreation;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.CategoryAggregate;
using MediatR;

public static class CreateCategory
{
    public record Command(string Name, string? Note, bool RequireNote) : IRequest;

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext dbContext;

        public Handler(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            Category category = new(name: command.Name, note: command.Note, requireNote: command.RequireNote);
            await dbContext.AddAsync(entity: category, cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
