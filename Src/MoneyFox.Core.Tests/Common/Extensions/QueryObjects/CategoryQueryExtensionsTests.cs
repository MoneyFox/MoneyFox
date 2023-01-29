namespace MoneyFox.Core.Tests.Common.Extensions.QueryObjects;

using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.Common.Extensions.QueryObjects;

public class CategoryQueryExtensionsTests
{
    [Fact]
    public void NameContains()
    {
        // Arrange
        var categoryQueryList = new List<Category> { new("Foo1"), new("Foo2"), new("absd") };

        // Act
        var resultList = categoryQueryList.WhereNameContains("Foo").ToList();

        // Assert
        Assert.Equal(expected: 2, actual: resultList.Count);
        Assert.Equal(expected: "Foo1", actual: resultList[0].Name);
        Assert.Equal(expected: "Foo2", actual: resultList[1].Name);
    }

    [Fact]
    public void OrderByName()
    {
        // Arrange
        var categoryQueryList = new List<Category> { new("Foo2"), new("Foo3"), new("Foo1") }.AsQueryable();

        // Act
        var resultList = categoryQueryList.OrderByName().ToList();

        // Assert
        Assert.Equal(expected: 3, actual: resultList.Count);
        Assert.Equal(expected: "Foo1", actual: resultList[0].Name);
        Assert.Equal(expected: "Foo2", actual: resultList[1].Name);
        Assert.Equal(expected: "Foo3", actual: resultList[2].Name);
    }
}
