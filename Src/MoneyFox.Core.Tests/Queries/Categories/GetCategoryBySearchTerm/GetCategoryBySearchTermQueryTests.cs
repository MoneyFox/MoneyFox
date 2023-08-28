﻿namespace MoneyFox.Core.Tests.Queries.Categories.GetCategoryBySearchTerm;

using Core.Queries;
using Domain.Aggregates.CategoryAggregate;

public class GetCategoryBySearchTermQueryTests : InMemoryTestBase
{
    private readonly GetCategoryBySearchTermQuery.Handler handler;

    public GetCategoryBySearchTermQueryTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetExcludedAccountQuery_WithoutFilter_CorrectNumberLoaded()
    {
        // Arrange
        var category1 = new Category("test");
        var category2 = new Category("test2");
        await Context.AddAsync(category1);
        await Context.AddAsync(category2);
        await Context.SaveChangesAsync();

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
        await Context.AddAsync(category1);
        await Context.AddAsync(category2);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new("guid"), cancellationToken: default);

        // Assert
        Assert.Single(result);
    }
}
