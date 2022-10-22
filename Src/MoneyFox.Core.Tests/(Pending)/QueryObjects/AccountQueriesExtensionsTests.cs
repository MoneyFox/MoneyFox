namespace MoneyFox.Core.Tests._Pending_.QueryObjects
{

    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using MoneyFox.Core._Pending_.Common.QueryObjects;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class AccountQueriesExtensionsTests
    {
        [Fact]
        public void AreActive()
        {
            // Arrange
            var accountQueryList = new List<Account> { new Account("Foo1"), new Account("Foo2"), new Account("absd") };
            accountQueryList[1].Deactivate();

            // Act
            var resultList = accountQueryList.AsQueryable().AreActive().ToList();

            // Assert
            Assert.Equal(expected: 2, actual: resultList.Count);
            Assert.Equal(expected: "Foo1", actual: resultList[0].Name);
            Assert.Equal(expected: "absd", actual: resultList[1].Name);
        }
    }

}
