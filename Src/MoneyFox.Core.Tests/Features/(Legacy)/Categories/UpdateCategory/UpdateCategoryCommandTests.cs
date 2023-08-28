namespace MoneyFox.Core.Tests.Features._Legacy_.Categories.UpdateCategory;

using Core.Features._Legacy_.Categories.UpdateCategory;
using Domain.Aggregates.CategoryAggregate;
using Microsoft.EntityFrameworkCore;

public class UpdateCategoryCommandTests : InMemoryTestBase
{
    private readonly UpdateCategory.Handler handler;

    public UpdateCategoryCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task UpdateCategoryCommand_CorrectNameLoaded()
    {
        // Arrange
        var category = new Category("test");
        await Context.AddAsync(category);
        await Context.SaveChangesAsync();

        // Act
        category.UpdateData("foo");
        await handler.Handle(
            command: new(Id: category.Id, Name: category.Name, Note: category.Note, RequireNote: category.RequireNote),
            cancellationToken: default);

        // Assert
        var loadedCategory = await Context.Categories.SingleAsync(c => c.Id == category.Id);
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
        await handler.Handle(
            command: new(Id: category.Id, Name: category.Name, Note: category.Note, RequireNote: category.RequireNote),
            cancellationToken: default);

        // Assert
        var loadedCategory = await Context.Categories.SingleAsync(c => c.Id == category.Id);
        loadedCategory.RequireNote.Should().BeTrue();
    }
}
