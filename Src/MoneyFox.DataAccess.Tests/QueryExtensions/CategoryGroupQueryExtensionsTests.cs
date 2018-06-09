using System.Collections.Generic;
using System.Linq;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.QueryExtensions;
using Xunit;

namespace MoneyFox.DataAccess.Tests.QueryExtensions
{
    public class CategoryGroupQueryExtensionsTests
    {
        [Fact]
        public void NameNotNull()
        {
            // Arrange
            var groupQuery = new List<CategoryGroupEntity>
                {
                    new CategoryGroupEntity {Id = 1, Name = null},
                    new CategoryGroupEntity {Id = 2, Name = null},
                    new CategoryGroupEntity {Id = 3, Name = "Rent"}
                }
                .AsQueryable();

            // Act
            var resultList = groupQuery.NameNotNull().ToList();

            // Assert
            Assert.Single(resultList);
            Assert.Equal(3, resultList[0].Id);
        }
        
        [Fact]
        public void NameEquals()
        {
            // Arrange
            var groupQuery = new List<CategoryGroupEntity>
                {
                    new CategoryGroupEntity {Id = 1, Name = "abc123de"},
                    new CategoryGroupEntity {Id = 2, Name = "123"},
                    new CategoryGroupEntity {Id = 3, Name = "Rent"}
                }
                .AsQueryable();

            // Act
            var resultList = groupQuery.NameEquals("123").ToList();

            // Assert
            Assert.Single(resultList);
            Assert.Equal(1, resultList[0].Id);
        }

        [Fact]
        public void SelectCategories()
        {
            // Arrange
            var groupQuery = new List<CategoryGroupEntity>
                {
                    new CategoryGroupEntity {Id = 1},
                    new CategoryGroupEntity {Id = 2}
                }
                .AsQueryable();

            // Act
            var resultList = groupQuery.SelectGroup().ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Data.Id);
            Assert.Equal(2, resultList[1].Data.Id);
        }

        [Fact]
        public void NameContainsOrderByName()
        {
            // Arrange
            var groupQuery = new List<CategoryGroupEntity>
                {
                    new CategoryGroupEntity {Id = 1},
                    new CategoryGroupEntity {Id = 2}
                }
                .AsQueryable();

            // Act
            var resultList = groupQuery.OrderByName().ToList();

            // Assert
            Assert.Equal(3, resultList.Count);
            Assert.Equal(3, resultList[0].Id);
            Assert.Equal(1, resultList[1].Id);
            Assert.Equal(2, resultList[2].Id);
        }
    }
}
