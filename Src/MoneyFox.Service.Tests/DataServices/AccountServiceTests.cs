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
    public class AccountServiceTests
    {
        #region GetExcludedAccounts

        [Fact]
        public async void GetAllAccounts_NoData_NoException()
        {
            // Arrange
            var data = new List<AccountEntity>().AsQueryable();

            var mockSet = new Mock<DbSet<AccountEntity>>();

            mockSet.As<IAsyncEnumerable<AccountEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(new TestAsyncEnumerator<AccountEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<AccountEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<AccountEntity>(data.Provider));

            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var contextOptions = new DbContextOptions<ApplicationContext>();
            var mockContext = new Mock<ApplicationContext>(contextOptions);
            mockContext.Setup(c => c.Set<AccountEntity>()).Returns(mockSet.Object);

            var dbFactory = new Mock<IDbFactory>();
            dbFactory.Setup(x => x.Init()).ReturnsAsync(mockContext.Object);

            var repository = new AccountRepository(dbFactory.Object);

            var contactDataService = new AccountService(repository, new Mock<IUnitOfWork>().Object);

            // Act
            var result = await contactDataService.GetAllAccounts();
            var resultList = result.ToList();

            // Assert
            Assert.Equal(0, resultList.Count);
        }

        public async void GetAllAccounts_AllDataReturned()
        {
            // Arrange
            var data = new List<AccountEntity>
            {
                new AccountEntity(),
                new AccountEntity{IsExcluded = true},
                new AccountEntity{IsOverdrawn = true},
                new AccountEntity{Name = "Income"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<AccountEntity>>();

            mockSet.As<IAsyncEnumerable<AccountEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(new TestAsyncEnumerator<AccountEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<AccountEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<AccountEntity>(data.Provider));

            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var contextOptions = new DbContextOptions<ApplicationContext>();
            var mockContext = new Mock<ApplicationContext>(contextOptions);
            mockContext.Setup(c => c.Set<AccountEntity>()).Returns(mockSet.Object);

            var dbFactory = new Mock<IDbFactory>();
            dbFactory.Setup(x => x.Init()).ReturnsAsync(mockContext.Object);

            var repository = new AccountRepository(dbFactory.Object);

            var contactDataService = new AccountService(repository, new Mock<IUnitOfWork>().Object);

            // Act
            var result = await contactDataService.GetAllAccounts();
            var resultList = result.ToList();

            // Assert
            Assert.Equal(4, resultList.Count);
            Assert.True(resultList[1].Data.IsExcluded);
            Assert.True(resultList[2].Data.IsOverdrawn);
            Assert.Equal("Income", resultList[3].Data.Name);
        }

        #endregion

        #region GetById

        [Theory]
        [InlineData(1, "Account1")]
        [InlineData(2, "Account2")]
        [InlineData(3, "Account3")]
        public async void GetById_NoData_NoException(int id, string expectedName)
        {
            // Arrange
            var data = new List<AccountEntity>
            {
                new AccountEntity{Id = 1, Name = "Account1"},
                new AccountEntity{Id = 2, Name = "Account2"},
                new AccountEntity{Id = 3, Name = "Account3"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<AccountEntity>>();

            mockSet.As<IAsyncEnumerable<AccountEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(new TestAsyncEnumerator<AccountEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<AccountEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<AccountEntity>(data.Provider));

            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var contextOptions = new DbContextOptions<ApplicationContext>();
            var mockContext = new Mock<ApplicationContext>(contextOptions);
            mockContext.Setup(c => c.Set<AccountEntity>()).Returns(mockSet.Object);

            var dbFactory = new Mock<IDbFactory>();
            dbFactory.Setup(x => x.Init()).ReturnsAsync(mockContext.Object);

            var repository = new AccountRepository(dbFactory.Object);

            var contactDataService = new AccountService(repository, new Mock<IUnitOfWork>().Object);

            // Act
            var result = await contactDataService.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedName, result.Data.Name);
        }

        #endregion

        #region GetAccountCount

        [Fact]
        public async void GetAccountCount_NoData_NoException()
        {
            // Arrange
            var data = new List<AccountEntity>
            {
                new AccountEntity{Id = 1, Name = "Account1"},
                new AccountEntity{Id = 2, Name = "Account2"},
                new AccountEntity{Id = 3, Name = "Account3"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<AccountEntity>>();

            mockSet.As<IAsyncEnumerable<AccountEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(new TestAsyncEnumerator<AccountEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<AccountEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<AccountEntity>(data.Provider));

            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var contextOptions = new DbContextOptions<ApplicationContext>();
            var mockContext = new Mock<ApplicationContext>(contextOptions);
            mockContext.Setup(c => c.Set<AccountEntity>()).Returns(mockSet.Object);

            var dbFactory = new Mock<IDbFactory>();
            dbFactory.Setup(x => x.Init()).ReturnsAsync(mockContext.Object);

            var repository = new AccountRepository(dbFactory.Object);

            var contactDataService = new AccountService(repository, new Mock<IUnitOfWork>().Object);

            // Act
            var result = await contactDataService.GetAccountCount();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async void GetAccountCount_CorrectAmount()
        {
            // Arrange
            var data = new List<AccountEntity>().AsQueryable();

            var mockSet = new Mock<DbSet<AccountEntity>>();

            mockSet.As<IAsyncEnumerable<AccountEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(new TestAsyncEnumerator<AccountEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<AccountEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<AccountEntity>(data.Provider));

            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var contextOptions = new DbContextOptions<ApplicationContext>();
            var mockContext = new Mock<ApplicationContext>(contextOptions);
            mockContext.Setup(c => c.Set<AccountEntity>()).Returns(mockSet.Object);

            var dbFactory = new Mock<IDbFactory>();
            dbFactory.Setup(x => x.Init()).ReturnsAsync(mockContext.Object);

            var repository = new AccountRepository(dbFactory.Object);

            var contactDataService = new AccountService(repository, new Mock<IUnitOfWork>().Object);

            // Act
            var result = await contactDataService.GetAccountCount();

            // Assert
            Assert.Equal(3, result);
        }

        #endregion

        #region CheckIfNameAlreadyTaken

        [Fact]
        public async void CheckIfNameAlreadyTaken_NoData_NoException()
        {
            // Arrange
            var data = new List<AccountEntity>().AsQueryable();

            var mockSet = new Mock<DbSet<AccountEntity>>();

            mockSet.As<IAsyncEnumerable<AccountEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(new TestAsyncEnumerator<AccountEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<AccountEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<AccountEntity>(data.Provider));

            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var contextOptions = new DbContextOptions<ApplicationContext>();
            var mockContext = new Mock<ApplicationContext>(contextOptions);
            mockContext.Setup(c => c.Set<AccountEntity>()).Returns(mockSet.Object);

            var dbFactory = new Mock<IDbFactory>();
            dbFactory.Setup(x => x.Init()).ReturnsAsync(mockContext.Object);

            var repository = new AccountRepository(dbFactory.Object);

            var contactDataService = new AccountService(repository, new Mock<IUnitOfWork>().Object);

            // Act
            var result = await contactDataService.CheckIfNameAlreadyTaken("Name");

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("Income", true)]
        [InlineData("Foo", false)]
        public async void CheckIfNameAlreadyTaken_CorrectlyEvaluated(string nameToCheck, bool expectedResult)
        {
            // Arrange
            var data = new List<AccountEntity>
            {
                new AccountEntity{Name = "Income"}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<AccountEntity>>();

            mockSet.As<IAsyncEnumerable<AccountEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(new TestAsyncEnumerator<AccountEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<AccountEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<AccountEntity>(data.Provider));

            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var contextOptions = new DbContextOptions<ApplicationContext>();
            var mockContext = new Mock<ApplicationContext>(contextOptions);
            mockContext.Setup(c => c.Set<AccountEntity>()).Returns(mockSet.Object);

            var dbFactory = new Mock<IDbFactory>();
            dbFactory.Setup(x => x.Init()).ReturnsAsync(mockContext.Object);

            var repository = new AccountRepository(dbFactory.Object);

            var contactDataService = new AccountService(repository, new Mock<IUnitOfWork>().Object);

            // Act
            var result = await contactDataService.CheckIfNameAlreadyTaken(nameToCheck);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region GetExcludedAccounts

        [Fact]
        public async void GetExcludedAccounts_NoData_NoException()
        {
            // Arrange
            var data = new List<AccountEntity>().AsQueryable();

            var mockSet = new Mock<DbSet<AccountEntity>>();

            mockSet.As<IAsyncEnumerable<AccountEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(new TestAsyncEnumerator<AccountEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<AccountEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<AccountEntity>(data.Provider));

            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var contextOptions = new DbContextOptions<ApplicationContext>();
            var mockContext = new Mock<ApplicationContext>(contextOptions);
            mockContext.Setup(c => c.Set<AccountEntity>()).Returns(mockSet.Object);

            var dbFactory = new Mock<IDbFactory>();
            dbFactory.Setup(x => x.Init()).ReturnsAsync(mockContext.Object);

            var repository = new AccountRepository(dbFactory.Object);

            var contactDataService = new AccountService(repository, new Mock<IUnitOfWork>().Object); 

            // Act
            var result = await contactDataService.GetExcludedAccounts();
            var resultList = result.ToList();

            // Assert
            Assert.Equal(0, resultList.Count);
        }

        #endregion

        #region GetExcludedAccounts

        [Fact]
        public async void GetNotExcludedAccounts_NoData_NoException()
        {
            // Arrange
            var data = new List<AccountEntity>().AsQueryable();

            var mockSet = new Mock<DbSet<AccountEntity>>();

            mockSet.As<IAsyncEnumerable<AccountEntity>>()
                   .Setup(m => m.GetEnumerator())
                   .Returns(new TestAsyncEnumerator<AccountEntity>(data.GetEnumerator()));

            mockSet.As<IQueryable<AccountEntity>>()
                   .Setup(m => m.Provider)
                   .Returns(new TestAsyncQueryProvider<AccountEntity>(data.Provider));

            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<AccountEntity>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var contextOptions = new DbContextOptions<ApplicationContext>();
            var mockContext = new Mock<ApplicationContext>(contextOptions);
            mockContext.Setup(c => c.Set<AccountEntity>()).Returns(mockSet.Object);

            var dbFactory = new Mock<IDbFactory>();
            dbFactory.Setup(x => x.Init()).ReturnsAsync(mockContext.Object);

            var repository = new AccountRepository(dbFactory.Object);

            var contactDataService = new AccountService(repository, new Mock<IUnitOfWork>().Object);

            // Act
            var result = await contactDataService.GetNotExcludedAccounts();
            var resultList = result.ToList();

            // Assert
            Assert.Equal(0, resultList.Count);
        }

        #endregion
    }
}
