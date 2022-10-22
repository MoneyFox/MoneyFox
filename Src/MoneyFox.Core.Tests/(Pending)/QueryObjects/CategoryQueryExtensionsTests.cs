namespace MoneyFox.Core.Tests._Pending_.QueryObjects
{

    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using MoneyFox.Core._Pending_.Common.QueryObjects;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class CategoryQueryExtensionsTests
    {
        [Fact]
        public void NameContains()
        {
            // Arrange
            var categoryQueryList = new List<Category> { new Category("Foo1"), new Category("Foo2"), new Category("absd") };

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
            var categoryQueryList = new List<Category> { new Category("Foo2"), new Category("Foo3"), new Category("Foo1") }.AsQueryable();

            // Act
            var resultList = categoryQueryList.OrderByName().ToList();

            // Assert
            Assert.Equal(expected: 3, actual: resultList.Count);
            Assert.Equal(expected: "Foo1", actual: resultList[0].Name);
            Assert.Equal(expected: "Foo2", actual: resultList[1].Name);
            Assert.Equal(expected: "Foo3", actual: resultList[2].Name);
        }
    }

}
