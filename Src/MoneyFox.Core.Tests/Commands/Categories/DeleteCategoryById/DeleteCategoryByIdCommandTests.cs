namespace MoneyFox.Core.Tests.Commands.Categories.DeleteCategoryById;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.Commands.Categories.DeleteCategoryById;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

[ExcludeFromCodeCoverage]
public class DeleteCategoryByIdCommandTests : InMemoryTestBase
{
    private readonly DeleteCategoryByIdCommand.Handler handler;

    public DeleteCategoryByIdCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task DeleteCategoryWithPassedId()
    {
        // Arrange
        var category1 = new Category("test");
        await Context.AddAsync(category1);
        await Context.SaveChangesAsync();

        // Act
        await handler.Handle(request: new(category1.Id), cancellationToken: default);

        // Assert
        (await Context.Categories.FirstOrDefaultAsync(x => x.Id == category1.Id)).Should().BeNull();
    }

    [Fact]
    public async Task DoesNothingWhenCategoryNotFound()
    {
        // Arrange
        var category1 = new Category("test");
        await Context.AddAsync(category1);
        await Context.SaveChangesAsync();

        // Act
        await handler.Handle(request: new(99), cancellationToken: default);

        // Assert
        (await Context.Categories.FirstOrDefaultAsync(x => x.Id == category1.Id)).Should().NotBeNull();
    }
}
