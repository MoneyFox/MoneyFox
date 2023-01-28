namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Categories.GetIfCategoryWithNameExists;

using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.ApplicationCore.Queries;
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
        var result = await new GetIfCategoryWithNameExistsQuery.Handler(Context).Handle(request: new("Foo"), cancellationToken: default);

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
        var result = await new GetIfCategoryWithNameExistsQuery.Handler(Context).Handle(request: new(testCat1.Name), cancellationToken: default);

        // Assert
        result.Should().BeTrue();
    }
}
