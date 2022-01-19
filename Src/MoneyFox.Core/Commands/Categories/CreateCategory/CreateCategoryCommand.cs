using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates.Payments;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyFox.Core.Commands.Categories.CreateCategory
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

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            /// <inheritdoc />
            public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = new Category(request.Name, request.Note, request.RequireNote);
                await contextAdapter.Context.Categories.AddAsync(category, cancellationToken);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}