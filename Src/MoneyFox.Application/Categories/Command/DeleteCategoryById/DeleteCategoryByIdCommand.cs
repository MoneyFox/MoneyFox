using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Interfaces;

namespace MoneyFox.Application.Categories.Command.DeleteCategoryById
{
    public class DeleteCategoryByIdCommand : IRequest
    {
        public int CategoryId { get; set; }

        public class Handler : IRequestHandler<DeleteCategoryByIdCommand> 
        {
            private readonly IEfCoreContext context;

            public Handler(IEfCoreContext context) {
                this.context = context;
            }

            public async Task<Unit> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken) 
            {
                var entityToDelete = await context.Categories.FindAsync(request.CategoryId, cancellationToken);

                context.Categories.Remove(entityToDelete);
                await context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
