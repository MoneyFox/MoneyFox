namespace MoneyFox.Core.Features._Legacy_.Categories.UpdateCategory;

using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using Domain.Aggregates.CategoryAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateCategoryCommand : IRequest
{
    public UpdateCategoryCommand(Category category)
    {
        Category = category;
    }

    public Category Category { get; }

    public class Handler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingCategory = await appDbContext.Categories.SingleAsync(predicate: c => c.Id == request.Category.Id, cancellationToken: cancellationToken);
            existingCategory.UpdateData(name: request.Category.Name, note: request.Category.Note ?? "", requireNote: request.Category.RequireNote);
            _ = await appDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
