namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Categories.GetCategoryBySearchTerm;

using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;

public class GetCategoryBySearchTermQueryTests
{
    private readonly AppDbContext context;
    private readonly GetCategoryBySearchTermQuery.Handler handler;

    public GetCategoryBySearchTermQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task GetExcludedAccountQuery_WithoutFilter_CorrectNumberLoaded()
    {
        // Arrange
        var category1 = new Category("test");
        var category2 = new Category("test2");
        await context.AddAsync(category1);
        await context.AddAsync(category2);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetExcludedAccountQuery_CorrectNumberLoaded()
    {
        // Arrange
        var category1 = new Category("this is a guid");
        var category2 = new Category("test2");
        await context.AddAsync(category1);
        await context.AddAsync(category2);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new("guid"), cancellationToken: default);

        // Assert
        Assert.Single(result);
    }
}
