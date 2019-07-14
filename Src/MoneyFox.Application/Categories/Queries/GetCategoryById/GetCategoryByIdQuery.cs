using MediatR;
using MoneyFox.Domain.Entities;

namespace MoneyFox.Application.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<Category>
    {
        public int CategoryId { get; set; }
    }
}
