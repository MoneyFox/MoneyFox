namespace MoneyFox.Core.Commands.Categories.CreateCategory
{

    using System.Threading;
    using System.Threading.Tasks;
    using Aggregates.Payments;
    using Common.Interfaces;
    using MediatR;

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

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            /// <inheritdoc />
            public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = new Category(name: request.Name, note: request.Note, requireNote: request.RequireNote);
                await contextAdapter.Context.Categories.AddAsync(entity: category, cancellationToken: cancellationToken);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }

}
