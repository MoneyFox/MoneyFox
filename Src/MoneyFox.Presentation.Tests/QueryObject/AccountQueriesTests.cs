using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable.Moq;
using MoneyFox.Presentation.QueryObject;
using MoneyFox.Presentation.ViewModels;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.QueryObject
{
    [ExcludeFromCodeCoverage]
    public class AccountQueriesTests
    {
        [Fact]
        public void OrderByName()
        {
            // Arrange
            IQueryable<AccountViewModel> accountQueryList = new List<AccountViewModel>
                {
                    new AccountViewModel {Name = "Foo2"},
                    new AccountViewModel {Name = "Foo3"},
                    new AccountViewModel {Name = "Foo1"}
                }
                .AsQueryable();

            // Act
            List<AccountViewModel> resultList = accountQueryList.OrderByName().ToList();

            // Assert
            Assert.Equal(3, resultList.Count);
            Assert.Equal("Foo1", resultList[0].Name);
            Assert.Equal("Foo2", resultList[1].Name);
            Assert.Equal("Foo3", resultList[2].Name);
        }

        [Theory]
        [InlineData("Foo3", true)]
        [InlineData("Foo5", false)]
        [InlineData("Foo", false)]
        [InlineData("abc", false)]
        public async Task AnyWithName(string searchName, bool expectedResult)
        {
            // Arrange
            IQueryable<AccountViewModel> accountQueryList = new List<AccountViewModel>
                                                            {
                                                                new AccountViewModel {Name = "Foo2"},
                                                                new AccountViewModel {Name = "Foo3"},
                                                                new AccountViewModel {Name = "Foo1"}
                                                            }
                                                            .AsQueryable()
                                                            .BuildMock()
                                                            .Object;

            // Act
            bool result = await accountQueryList.AnyWithNameAsync(searchName);

            // Assert
            result.ShouldEqual(expectedResult);
        }

        [Fact]
        public void AreNotExcluded()
        {
            // Arrange
            IQueryable<AccountViewModel> accountQueryList = new List<AccountViewModel>
                {
                    new AccountViewModel {Id = 1, IsExcluded = false},
                    new AccountViewModel {Id = 2, IsExcluded = true},
                    new AccountViewModel {Id = 3, IsExcluded = false}
                }
                .AsQueryable();

            // Act
            List<AccountViewModel> resultList = accountQueryList.AreNotExcluded().ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal(3, resultList[1].Id);
        }

        [Fact]
        public void AreExcluded()
        {
            // Arrange
            IQueryable<AccountViewModel> accountQueryList = new List<AccountViewModel>
                {
                    new AccountViewModel {Id = 1, IsExcluded = false},
                    new AccountViewModel {Id = 2, IsExcluded = true},
                    new AccountViewModel {Id = 3, IsExcluded = false}
                }
                .AsQueryable();

            // Act
            List<AccountViewModel> resultList = accountQueryList.AreExcluded().ToList();

            // Assert
            Assert.Single(resultList);
            Assert.Equal(2, resultList[0].Id);
        }
    }
}
