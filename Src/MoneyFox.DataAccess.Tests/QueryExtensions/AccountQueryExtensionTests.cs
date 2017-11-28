using System.Collections.Generic;
using System.Linq;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.QueryExtensions;
using Xunit;

namespace MoneyFox.DataAccess.Tests.QueryExtensions
{
    public class AccountQueryExtensionTests
    {
        [Fact]
        public void AreNotExcluded()
        {
            // Arrange
            var accountListQuery = new List<AccountEntity>
                {
                    new AccountEntity {Id = 1, IsExcluded = true},
                    new AccountEntity {Id = 2, IsExcluded = false},
                    new AccountEntity {Id = 3, IsExcluded = false}
                }
                .AsQueryable();

            // Act
            var resultList = accountListQuery.AreNotExcluded().ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(2, resultList[0].Id);
            Assert.Equal(3, resultList[1].Id);
        }

        [Fact]
        public void AreExcluded()
        {
            // Arrange
            var accountListQuery = new List<AccountEntity>
                {
                    new AccountEntity {Id = 1, IsExcluded = true},
                    new AccountEntity {Id = 2, IsExcluded = false},
                    new AccountEntity {Id = 3, IsExcluded = false}
                }
                .AsQueryable();

            // Act
            var resultList = accountListQuery.AreExcluded().ToList();

            // Assert
            Assert.Equal(1, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
        }

        [Fact]
        public void NameEquals()
        {
            // Arrange
            var accountListQuery = new List<AccountEntity>
                {
                    new AccountEntity {Id = 1, Name = "Foo"},
                    new AccountEntity {Id = 2, Name = "Income"},
                    new AccountEntity {Id = 3, Name = "SomethingElse"}
                }
                .AsQueryable();

            // Act
            var resultList = accountListQuery.NameEquals("Income").ToList();

            // Assert
            Assert.Equal(1, resultList.Count);
            Assert.Equal(2, resultList[0].Id);
        }

        [Fact]
        public void OrderByName()
        {
            // Arrange
            var accountListQuery = new List<AccountEntity>
                {
                    new AccountEntity {Id = 1, Name = "Foo"},
                    new AccountEntity {Id = 2, Name = "Alber"},
                    new AccountEntity {Id = 3, Name = "Thom"}
                }
                .AsQueryable();

            // Act
            var resultList = accountListQuery.OrderByName().ToList();

            // Assert
            Assert.Equal(3, resultList.Count);
            Assert.Equal(2, resultList[0].Id);
            Assert.Equal(1, resultList[1].Id);
            Assert.Equal(3, resultList[2].Id);
        }

        [Fact]
        public void SelectAccounts()
        {
            // Arrange
            var accountListQuery = new List<AccountEntity>
                {
                    new AccountEntity {Id = 1},
                    new AccountEntity {Id = 2}
                }
                .AsQueryable();

            // Act
            var resultList = accountListQuery.SelectAccounts().ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Data.Id);
            Assert.Equal(2, resultList[1].Data.Id);
        }
    }
}
