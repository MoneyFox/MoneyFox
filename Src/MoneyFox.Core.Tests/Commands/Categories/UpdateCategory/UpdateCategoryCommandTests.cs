namespace MoneyFox.Core.Tests.Commands.Categories.UpdateCategory;

using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.Commands.Categories.UpdateCategory;
using FluentAssertions;
using Infrastructure.Persistence;

public class UpdateCategoryCommandTests : InMemoryTestBase
{
    private readonly UpdateCategoryCommand.Handler handler;

    public UpdateCategoryCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task UpdateCategoryCommand_CorrectNumberLoaded()
    {
        // Arrange
        var category = new Category("test");
        await Context.AddAsync(category);
        await Context.SaveChangesAsync();

        // Act
        category.UpdateData("foo");
        await handler.Handle(request: new(category), cancellationToken: default);
        var loadedCategory = await Context.Categories.FindAsync(category.Id);

        // Assert
        loadedCategory.Name.Should().Be("foo");
    }

    [Fact]
    public async Task UpdateCategoryCommand_ShouldUpdateRequireNote()
    {
        // Arrange
        var category = new Category(name: "test", requireNote: false);
        await Context.AddAsync(category);
        await Context.SaveChangesAsync();

        // Act
        category.UpdateData(name: "foo", requireNote: true);
        await handler.Handle(request: new(category), cancellationToken: default);
        var loadedCategory = await Context.Categories.FindAsync(category.Id);

        // Assert
        loadedCategory.RequireNote.Should().BeTrue();
    }
}
