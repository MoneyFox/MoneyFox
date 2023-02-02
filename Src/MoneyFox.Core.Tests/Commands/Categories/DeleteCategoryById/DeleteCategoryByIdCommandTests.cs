namespace MoneyFox.Core.Tests.Commands.Categories.DeleteCategoryById;

using Core.Features._Legacy_.Categories.DeleteCategoryById;
using Domain.Aggregates.CategoryAggregate;
using Domain.Tests.TestFramework;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

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
        var testCategory = new TestData.DefaultCategory();
        Context.RegisterCategory(testCategory:testCategory);

        // Act
        await handler.Handle(request: new(testCategory.Id), cancellationToken: default);

        // Assert
        (await Context.Categories.FirstOrDefaultAsync(x => x.Id == testCategory.Id)).Should().BeNull();
    }

    [Fact]
    public async Task DoesNothingWhenCategoryNotFound()
    {
        // Arrange
        var testCategory = new TestData.DefaultCategory();
        Context.RegisterCategory(testCategory:testCategory);

        // Act
        await handler.Handle(request: new(99), cancellationToken: default);

        // Assert
        (await Context.Categories.FirstOrDefaultAsync(x => x.Id == testCategory.Id)).Should().NotBeNull();
    }
}
