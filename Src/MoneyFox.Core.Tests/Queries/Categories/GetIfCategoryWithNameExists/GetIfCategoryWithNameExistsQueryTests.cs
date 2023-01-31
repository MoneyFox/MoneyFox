namespace MoneyFox.Core.Tests.Queries.Categories.GetIfCategoryWithNameExists;

using Core.Queries;
using Domain.Aggregates.CategoryAggregate;
using FluentAssertions;

public class GetIfCategoryWithNameExistsQueryTests : InMemoryTestBase
{
    [Fact]
    public async Task CategoryWithSameNameDontExist()
    {
        // Arrange
        var testCat1 = new Category("Ausgehen");
        await Context.Categories.AddAsync(testCat1);
        await Context.SaveChangesAsync();

        // Act
        var result = await new GetIfCategoryWithNameExistsQuery.Handler(Context).Handle(request: new("Foo", 0), cancellationToken: default);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CategoryWithSameNameExist()
    {
        // Arrange
        var testCat1 = new Category("Ausgehen");
        await Context.Categories.AddAsync(testCat1);
        await Context.SaveChangesAsync();

        // Act
        var result = await new GetIfCategoryWithNameExistsQuery.Handler(Context).Handle(request: new(testCat1.Name, 0), cancellationToken: default);

        // Assert
        result.Should().BeTrue();
    }
}
