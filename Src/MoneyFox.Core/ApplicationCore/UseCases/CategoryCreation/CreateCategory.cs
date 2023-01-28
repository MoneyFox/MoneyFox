namespace MoneyFox.Core.ApplicationCore.UseCases.CategoryCreation;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.CategoryAggregate;
using MediatR;

public static class CreateCategory
{
    public sealed class Command : IRequest
    {
        public Command(string name, string? note = null, bool requireNote = false)
        {
            Name = name;
            Note = note;
            RequireNote = requireNote;
        }

        public string Name { get; }
        public string? Note { get; }
        public bool RequireNote { get; }
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            Category category = new(name: request.Name, note: request.Note, requireNote: request.RequireNote);
            await appDbContext.AddAsync(entity: category, cancellationToken: cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
