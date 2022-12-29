namespace MoneyFox.Core.Tests.Commands.Categories.DeleteCategoryById;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.Commands.Categories.DeleteCategoryById;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using TestFramework;

[ExcludeFromCodeCoverage]
public class DeleteCategoryByIdCommandTests
{
    private readonly AppDbContext context;
    private readonly DeleteCategoryByIdCommand.Handler handler;

    public DeleteCategoryByIdCommandTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task GetExcludedAccountQuery_WithoutFilter_CorrectNumberLoaded()
    {
        // Arrange
        var category1 = new Category("test");
        await context.AddAsync(category1);
        await context.SaveChangesAsync();

        // Act
        await handler.Handle(request: new(category1.Id), cancellationToken: default);

        // Assert
        (await context.Categories.FirstOrDefaultAsync(x => x.Id == category1.Id)).Should().BeNull();
    }
}
