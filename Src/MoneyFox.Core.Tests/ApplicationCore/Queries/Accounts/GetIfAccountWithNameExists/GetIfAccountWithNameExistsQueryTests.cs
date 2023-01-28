namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts.GetIfAccountWithNameExists;

using Core.ApplicationCore.Queries;
using FluentAssertions;
using MoneyFox.Domain.Aggregates.AccountAggregate;

public class GetIfAccountWithNameExistsQueryTests : InMemoryTestBase
{
    private readonly GetIfAccountWithNameExistsQuery.Handler handler;

    public GetIfAccountWithNameExistsQueryTests()
    {
        handler = new(Context);
    }

    [Theory]
    [InlineData("Foo", true)]
    [InlineData("foo", true)]
    [InlineData("Foo212", false)]
    public async Task GetExcludedAccountQuery_CorrectNumberLoaded(string name, bool expectedResult)
    {
        // Arrange
        var accountExcluded = new Account(name: "Foo", initialBalance: 80, isExcluded: true);
        var accountIncluded = new Account(name: "test", initialBalance: 80);
        await Context.AddAsync(accountExcluded);
        await Context.AddAsync(accountIncluded);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(accountName: name, accountId: 0), cancellationToken: default);

        // Assert
        result.Should().Be(expectedResult);
    }
}
