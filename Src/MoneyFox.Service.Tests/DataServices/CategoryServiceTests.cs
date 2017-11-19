using System.Collections.Generic;
using System.Linq;
using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Entities;
using MoneyFox.Service.Tests.TestHelper;
using Moq;
using Xunit;

namespace MoneyFox.Service.Tests.DataServices
{
    public class CategoryServiceTests
    {
        [Fact]
        public async void GetAllCategories_NoData_NoException()
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

            var ambientDbContextLocatorMock = new Mock<IAmbientDbContextLocator>();
            ambientDbContextLocatorMock.Setup(x => x.Get<ApplicationContext>())
                                   .Returns(mockContext.Object);

            var contactDataService = new CategoryService(ambientDbContextLocatorMock.Object, new DbContextScopeFactory());

            // Act
            var result = await contactDataService.GetAllCategories();
            var resultList = result.ToList();

            // Assert
            Assert.Equal(0, resultList.Count);
        }

        [Fact]
        public async void GetAllCategories_AllCategoriesReturned()
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

            var ambientDbContextLocatorMock = new Mock<IAmbientDbContextLocator>();
            ambientDbContextLocatorMock.Setup(x => x.Get<ApplicationContext>())
                                       .Returns(mockContext.Object);

            var contactDataService = new CategoryService(ambientDbContextLocatorMock.Object, new DbContextScopeFactory());

            // Act
            var result = await contactDataService.GetAllCategories();
            var resultList = result.ToList();

            // Assert
            Assert.Equal(4, resultList.Count);
        }


        [Fact]
        public async void GetAllCategoriesWithPayments_NoData_NoException()
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

            var ambientDbContextLocatorMock = new Mock<IAmbientDbContextLocator>();
            ambientDbContextLocatorMock.Setup(x => x.Get<ApplicationContext>())
                                       .Returns(mockContext.Object);

            var contactDataService = new CategoryService(ambientDbContextLocatorMock.Object, new DbContextScopeFactory());

            // Act
            var result = await contactDataService.GetAllCategoriesWithPayments();
            var resultList = result.ToList();

            // Assert
            Assert.Empty(resultList);
        }

        [Fact]
        public async void GetAllCategoriesWithPayments_AllCategoriesReturned()
        {
            // Arrange
            var data = new List<CategoryEntity>
            {
                new CategoryEntity {Name = "Category1", Payments = new List<PaymentEntity> {new PaymentEntity()}},
                new CategoryEntity {Name = "Category2"},
                new CategoryEntity
                {
                    Name = "Category3",
                    Payments = new List<PaymentEntity> {new PaymentEntity(), new PaymentEntity()}
                }
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

            var ambientDbContextLocatorMock = new Mock<IAmbientDbContextLocator>();
            ambientDbContextLocatorMock.Setup(x => x.Get<ApplicationContext>())
                                       .Returns(mockContext.Object);

            var contactDataService = new CategoryService(ambientDbContextLocatorMock.Object, new DbContextScopeFactory());

            // Act
            var result = await contactDataService.GetAllCategories();
            var resultList = result.ToList();

            // Assert
            Assert.Equal(3, resultList.Count);
            Assert.Single(resultList[0].Data.Payments);
            Assert.Null(resultList[1].Data.Payments);
            Assert.Equal(2, resultList[2].Data.Payments.Count);
        }

        [Fact]
        public async void GetById_NoData_NoException()
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

            var ambientDbContextLocatorMock = new Mock<IAmbientDbContextLocator>();
            ambientDbContextLocatorMock.Setup(x => x.Get<ApplicationContext>())
                                       .Returns(mockContext.Object);

            var contactDataService = new CategoryService(ambientDbContextLocatorMock.Object, new DbContextScopeFactory());

            // Act
            var result = await contactDataService.GetById(3);

            // Assert
            Assert.Null(result.Data);
        }
    }
}
