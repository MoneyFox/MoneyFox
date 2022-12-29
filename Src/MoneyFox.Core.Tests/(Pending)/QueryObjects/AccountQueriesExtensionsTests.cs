namespace MoneyFox.Core.Tests._Pending_.QueryObjects;

using System.Diagnostics.CodeAnalysis;
using Core._Pending_.Common.QueryObjects;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;

[ExcludeFromCodeCoverage]
public class AccountQueriesExtensionsTests
{
    [Fact]
    public void AreActive()
    {
        // Arrange
        var accountQueryList = new List<Account> { new("Foo1"), new("Foo2"), new("absd") };
        accountQueryList[1].Deactivate();

        // Act
        var resultList = accountQueryList.AsQueryable().AreActive().ToList();

        // Assert
        Assert.Equal(expected: 2, actual: resultList.Count);
        Assert.Equal(expected: "Foo1", actual: resultList[0].Name);
        Assert.Equal(expected: "absd", actual: resultList[1].Name);
    }
}
