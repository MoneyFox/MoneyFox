using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MoneyFox.Application.Interfaces;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryViewModelByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
    {
        private readonly IEfCoreContext context;

        public GetCategoryViewModelByIdQueryHandler(IEfCoreContext context)
        {
            this.context = context;
        }

        public async Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.Categories.FindAsync(request.CategoryId);
        }
    }
}
