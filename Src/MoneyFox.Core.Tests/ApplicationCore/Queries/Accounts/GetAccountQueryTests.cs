namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;

public class GetAccountQueryTests : InMemoryTestBase
{
    private readonly GetAccountsQuery.Handler handler;

    public GetAccountQueryTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetAccountQuery_CorrectNumberLoaded()
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        await Context.AddAsync(account);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task DontLoadDeactivatedAccounts()
    {
        // Arrange
        var account1 = new Account(name: "test", initialBalance: 80);
        var account2 = new Account(name: "test", initialBalance: 80);
        account2.Deactivate();
        await Context.AddAsync(account1);
        await Context.AddAsync(account2);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        Assert.Single(result);
    }
}
