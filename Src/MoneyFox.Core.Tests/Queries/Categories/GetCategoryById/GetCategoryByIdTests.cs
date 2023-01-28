namespace MoneyFox.Core.Tests.Queries.Categories.GetCategoryById;

using Core.Queries;
using Domain.Aggregates.CategoryAggregate;
using FluentAssertions;

public class GetCategoryByIdTests : InMemoryTestBase
{
    private readonly GetCategoryById.Handler handler;

    public GetCategoryByIdTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetCategory_CategoryNotFound()
    {
        // Arrange

        // Act
        Func<Task<CategoryData>> act = () => handler.Handle(request: new(999), cancellationToken: default);

        // Assert
        _ = await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetCategory_CategoryFound()
    {
        // Arrange
        Category testCat = new(name: "Ausgehen", note: "My Note", requireNote: true);
        _ = await Context.Categories.AddAsync(testCat);
        _ = await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(testCat.Id), cancellationToken: default);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.Name.Should().Be(testCat.Name);
        _ = result.Note.Should().Be(testCat.Note);
        _ = result.NoteRequired.Should().Be(testCat.RequireNote);
        _ = result.Created.Should().Be(testCat.Created);
        _ = result.LastModified.Should().Be(testCat.LastModified);
    }
}
