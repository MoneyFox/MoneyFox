namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Categories.GetCategoryById;

using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;

public class GetCategoryByIdQueryTests
{
    private readonly AppDbContext context;
    private readonly GetCategoryByIdQuery.Handler handler;

    public GetCategoryByIdQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
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
        await context.Categories.AddAsync(testCat1);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(testCat1.Id), cancellationToken: default);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(testCat1.Name);
    }
}
