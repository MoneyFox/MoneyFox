using System.Collections.Generic;
using System.Linq;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.QueryExtensions;
using Xunit;

namespace MoneyFox.DataAccess.Tests.QueryExtensions
{
    public class CategoryQueryExtensionsTests
    {
        [Fact]
        public void NameNotNull()
        {
            // Arrange
            var categoryQueryList = new List<CategoryEntity>
                {
                    new CategoryEntity {Id = 1, Name = null},
                    new CategoryEntity {Id = 2, Name = null},
                    new CategoryEntity {Id = 3, Name = "Rent"}
                }
                .AsQueryable();

            // Act
            var resultList = categoryQueryList.NameNotNull().ToList();

            // Assert
            Assert.Equal(1, resultList.Count);
            Assert.Equal(3, resultList[0].Id);
        }

        [Fact]
        public void NameContains()
        {
            // Arrange
            var categoryQueryList = new List<CategoryEntity>
                {
                    new CategoryEntity {Id = 1, Name = "abc123de"},
                    new CategoryEntity {Id = 2, Name = "123"},
                    new CategoryEntity {Id = 3, Name = "Rent"}
                }
                .AsQueryable();

            // Act
            var resultList = categoryQueryList.NameContains("123").ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal(2, resultList[1].Id);
        }

        [Fact]
        public void SelectCategories()
        {
            // Arrange
            var categorieQuery = new List<CategoryEntity>
                {
                    new CategoryEntity {Id = 1},
                    new CategoryEntity {Id = 2}
                }
                .AsQueryable();

            // Act
            var resultList = categorieQuery.SelectCategories().ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Data.Id);
            Assert.Equal(2, resultList[1].Data.Id);
        }

        [Fact]
        public void NameContainsOrderByName()
        {
            // Arrange
            var categoryQueryList = new List<CategoryEntity>
                {
                    new CategoryEntity {Id = 1, Name = "c"},
                    new CategoryEntity {Id = 2, Name = "d"},
                    new CategoryEntity {Id = 3, Name = "a"}
                }
                .AsQueryable();

            // Act
            var resultList = categoryQueryList.OrderByName().ToList();

            // Assert
            Assert.Equal(3, resultList.Count);
            Assert.Equal(3, resultList[0].Id);
            Assert.Equal(1, resultList[1].Id);
            Assert.Equal(2, resultList[2].Id);
        }
    }
}
