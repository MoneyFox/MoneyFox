using System.Collections.Generic;
using System.Linq;
using MoneyFox.DataAccess.Entities;
using MoneyFox.Service.QueryExtensions;
using Xunit;

namespace MoneyFox.Service.Tests.QueryExtensions
{
    public class CategoryQueryExtensions
    {
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
    }
}
