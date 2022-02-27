namespace MoneyFox.Core.Commands.Categories.UpdateCategory
{
    using _Pending_.Common.Interfaces;
    using Aggregates.Payments;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

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
                Category existingCategory = await contextAdapter.Context.Categories.FindAsync(request.Category.Id);

                existingCategory.UpdateData(
                    request.Category.Name,
                    request.Category.Note ?? "",
                    request.Category.RequireNote);

                await contextAdapter.Context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}