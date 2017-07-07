using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Service.DataServices;
using MoneyFox.Service.Tests.TestHelper;
using Moq;
using Xunit;

namespace MoneyFox.Service.Tests.DataServices
{
    public class CategoryServiceTests
    {
        #region GetAllAccounts

        [Fact]
        public async void GetAllAccounts_NoData_NoException()
        {
            // Arrange
            var data = new List<CategoryEntity>().AsQueryable();

            var mockSet = new Mock<DbSet<CategoryEntity>>();

            mockSet.As<IAsyncEnumerable<CategoryEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(new TestAsyncEnumerator<CategoryEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<CategoryEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<CategoryEntity>(data.Provider));

            mockSet.As<IQueryable<CategoryEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<CategoryEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<CategoryEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var contextOptions = new DbContextOptions<ApplicationContext>();
            var mockContext = new Mock<ApplicationContext>(contextOptions);
            mockContext.Setup(c => c.Set<CategoryEntity>()).Returns(mockSet.Object);

            var dbFactory = new Mock<IDbFactory>();
            dbFactory.Setup(x => x.Init()).ReturnsAsync(mockContext.Object);

            var repository = new CategoryRepository(dbFactory.Object);

            var contactDataService = new CategoryService(repository, new Mock<IUnitOfWork>().Object);

            // Act
            var result = await contactDataService.GetAllCategories();
            var resultList = result.ToList();

            // Assert
            Assert.Equal(0, resultList.Count);
        }

        [Fact]
        public async void GetAllAccounts_AllCategoriesReturned()
        {
            // Arrange
            var data = new List<CategoryEntity>
            {
                new CategoryEntity{Name = "Category1"},
                new CategoryEntity{Name = "Category2"},
                new CategoryEntity{Name = "Category3"},
                new CategoryEntity{Name = "Category4"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<CategoryEntity>>();

            mockSet.As<IAsyncEnumerable<CategoryEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(new TestAsyncEnumerator<CategoryEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<CategoryEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<CategoryEntity>(data.Provider));

            mockSet.As<IQueryable<CategoryEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<CategoryEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<CategoryEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var contextOptions = new DbContextOptions<ApplicationContext>();
            var mockContext = new Mock<ApplicationContext>(contextOptions);
            mockContext.Setup(c => c.Set<CategoryEntity>()).Returns(mockSet.Object);

            var dbFactory = new Mock<IDbFactory>();
            dbFactory.Setup(x => x.Init()).ReturnsAsync(mockContext.Object);

            var repository = new CategoryRepository(dbFactory.Object);

            var contactDataService = new CategoryService(repository, new Mock<IUnitOfWork>().Object);

            // Act
            var result = await contactDataService.GetAllCategories();
            var resultList = result.ToList();

            // Assert
            Assert.Equal(4, resultList.Count);
        }

        #endregion
    }
}
