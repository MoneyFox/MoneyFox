namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts;

using Core.ApplicationCore.Queries;
using MoneyFox.Domain.Aggregates.AccountAggregate;

public class GetIncludedAccountQueryTests : InMemoryTestBase
{
    private readonly GetIncludedAccountQuery.Handler handler;

    public GetIncludedAccountQueryTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetIncludedAccountQuery_CorrectNumberLoaded()
    {
        // Arrange
        var accountExcluded = new Account(name: "test", initialBalance: 80, isExcluded: true);
        var accountIncluded = new Account(name: "test", initialBalance: 80);
        await Context.AddAsync(accountExcluded);
        await Context.AddAsync(accountIncluded);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task DontLoadDeactivatedAccount()
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
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        Assert.Single(result);
    }
}
