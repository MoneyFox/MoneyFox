namespace MoneyFox.Core.Commands.Categories.DeleteCategoryById
{
    using Aggregates.Payments;
    using Common.Interfaces;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteCategoryByIdCommand : IRequest
    {
        public DeleteCategoryByIdCommand(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; }

        public class Handler : IRequestHandler<DeleteCategoryByIdCommand>
        {
            private readonly IContextAdapter contextAdapter;

            public Handler(IContextAdapter contextAdapter)
            {
                this.contextAdapter = contextAdapter;
            }

            public async Task<Unit> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
            {
                Category entityToDelete = await contextAdapter.Context.Categories.FindAsync(request.CategoryId);

                contextAdapter.Context.Categories.Remove(entityToDelete);
                await contextAdapter.Context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}