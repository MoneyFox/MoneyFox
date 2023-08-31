namespace MoneyFox.Core.Tests.Queries.Categories.GetCategoryById;

using Core.Queries;
using Domain.Aggregates.CategoryAggregate;

public class GetCategoryByIdQueryTests : InMemoryTestBase
{
    private readonly GetCategoryByIdQuery.Handler handler;

    public GetCategoryByIdQueryTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetCategory_CategoryNotFound()
    {
        // Arrange

        // Act
        var act = () => handler.Handle(request: new(999), cancellationToken: default);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetCategory_CategoryFound()
    {
        // Arrange
        var testCat1 = new Category("Ausgehen");
        await Context.Categories.AddAsync(testCat1);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(testCat1.Id), cancellationToken: default);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(testCat1.Name);
    }
}
