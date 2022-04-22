namespace MoneyFox.Core.Commands.Categories.UpdateCategory
{

    using System.Threading;
    using System.Threading.Tasks;
    using ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using Common.Interfaces;
    using MediatR;

    public class UpdateCategoryCommand : IRequest
    {
        public UpdateCategoryCommand(Category category)
        {
            Category = category;
        }

        public Category Category { get; }

        public class Handler : IRequestHandler<UpdateCategoryCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                var existingCategory = await contextAdapter.Context.Categories.FindAsync(request.Category.Id);
                existingCategory.UpdateData(name: request.Category.Name, note: request.Category.Note ?? "", requireNote: request.Category.RequireNote);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }

}
