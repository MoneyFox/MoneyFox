namespace MoneyFox.Core.Tests.Commands.Categories.UpdateCategory;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.Commands.Categories.UpdateCategory;
using FluentAssertions;
using Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class UpdateCategoryCommandTests
{
    private readonly AppDbContext context;
    private readonly UpdateCategoryCommand.Handler handler;

    public UpdateCategoryCommandTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task UpdateCategoryCommand_CorrectNumberLoaded()
    {
        // Arrange
        var category = new Category("test");
        await context.AddAsync(category);
        await context.SaveChangesAsync();

        // Act
        category.UpdateData("foo");
        await handler.Handle(request: new(category), cancellationToken: default);
        var loadedCategory = await context.Categories.FindAsync(category.Id);

        // Assert
        loadedCategory.Name.Should().Be("foo");
    }

    [Fact]
    public async Task UpdateCategoryCommand_ShouldUpdateRequireNote()
    {
        // Arrange
        var category = new Category(name: "test", requireNote: false);
        await context.AddAsync(category);
        await context.SaveChangesAsync();

        // Act
        category.UpdateData(name: "foo", requireNote: true);
        await handler.Handle(request: new(category), cancellationToken: default);
        var loadedCategory = await context.Categories.FindAsync(category.Id);

        // Assert
        loadedCategory.RequireNote.Should().BeTrue();
    }
}
