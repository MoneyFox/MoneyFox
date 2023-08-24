namespace MoneyFox.Core.Tests.Queries.Categories.GetIfCategoryWithNameExists;

using Core.Queries;
using Domain.Aggregates.CategoryAggregate;

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
        var result = await new GetIfCategoryWithNameExistsQuery.Handler(Context).Handle(
            request: new(categoryName: "Foo", categoryId: 0),
            cancellationToken: default);

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
        var result = await new GetIfCategoryWithNameExistsQuery.Handler(Context).Handle(
            request: new(categoryName: testCat1.Name, categoryId: 0),
            cancellationToken: default);

        // Assert
        result.Should().BeTrue();
    }
}
