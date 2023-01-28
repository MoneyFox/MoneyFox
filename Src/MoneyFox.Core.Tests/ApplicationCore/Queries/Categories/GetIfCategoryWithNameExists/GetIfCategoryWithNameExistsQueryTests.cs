namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Categories.GetIfCategoryWithNameExists;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class GetIfCategoryWithNameExistsQueryTests : IDisposable
{
    private readonly AppDbContext context;

    public GetIfCategoryWithNameExistsQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        InMemoryAppDbContextFactory.Destroy(context);
    }

    [Fact]
    public async Task CategoryWithSameNameDontExist()
    {
        // Arrange
        var testCat1 = new Category("Ausgehen");
        await context.Categories.AddAsync(testCat1);
        await context.SaveChangesAsync();

        // Act
        var result = await new GetIfCategoryWithNameExistsQuery.Handler(context).Handle(request: new("Foo"), cancellationToken: default);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CategoryWithSameNameExist()
    {
        // Arrange
        var testCat1 = new Category("Ausgehen");
        await context.Categories.AddAsync(testCat1);
        await context.SaveChangesAsync();

        // Act
        var result = await new GetIfCategoryWithNameExistsQuery.Handler(context).Handle(request: new(testCat1.Name), cancellationToken: default);

        // Assert
        result.Should().BeTrue();
    }
}
