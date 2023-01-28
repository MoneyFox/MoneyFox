namespace MoneyFox.Core.Queries;

using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public record CategoryData(
    int Id,
    string Name,
    string? Note,
    bool NoteRequired,
    DateTime Created,
    DateTime? LastModified);

public static class GetCategoryById
{
    public record Query(int CategoryId) : IRequest<CategoryData>;

    public class Handler : IRequestHandler<Query, CategoryData>
    {
        private readonly IAppDbContext appDbContext;

        public Handler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<CategoryData> Handle(Query request, CancellationToken cancellationToken)
        {
            var category = await appDbContext.Categories.FirstAsync(predicate: c => c.Id == request.CategoryId, cancellationToken: cancellationToken);

            return new(
                Id: category.Id,
                Name: category.Name,
                Note: category.Note,
                NoteRequired: category.RequireNote,
                Created: category.Created,
                LastModified: category.LastModified);
        }
    }
}
