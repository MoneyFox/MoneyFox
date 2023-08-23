namespace MoneyFox.Core.Tests.Queries.Accounts;

using Core.Queries;
using Domain.Aggregates.AccountAggregate;

public class GetExcludedAccountQueryTests : InMemoryTestBase
{
    private readonly GetExcludedAccountQuery.Handler handler;

    public GetExcludedAccountQueryTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetExcludedAccountQuery_CorrectNumberLoaded()
    {
        // Arrange
        var accountExcluded = new Account(name: "test", initialBalance: 80, isExcluded: true);
        var accountIncluded = new Account(name: "test", initialBalance: 80);
        await Context.AddAsync(accountExcluded);
        await Context.AddAsync(accountIncluded);
        await Context.SaveChangesAsync();

        // Act
        var resultList = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        resultList.Should().ContainSingle();
        resultList[0].CurrentBalance.Should().Be(80);
    }

    [Fact]
    public async Task DontLoadDeactivatedAccounts()
    {
        // Arrange
        var accountExcluded = new Account(name: "test", initialBalance: 80, isExcluded: true);
        var accountIncluded = new Account(name: "test", initialBalance: 80);
        var accountDeactivated = new Account(name: "test", initialBalance: 80);
        accountDeactivated.Deactivate();
        await Context.AddAsync(accountExcluded);
        await Context.AddAsync(accountIncluded);
        await Context.AddAsync(accountDeactivated);
        await Context.SaveChangesAsync();

        // Act
        var resultList = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        resultList.Should().ContainSingle();
        resultList[0].CurrentBalance.Should().Be(80);
    }
}
