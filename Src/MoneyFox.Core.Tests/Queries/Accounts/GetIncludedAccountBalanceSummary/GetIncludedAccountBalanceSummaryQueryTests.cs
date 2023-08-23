namespace MoneyFox.Core.Tests.Queries.Accounts.GetIncludedAccountBalanceSummary;

using Core.Queries;
using Domain.Aggregates.AccountAggregate;

public class GetIncludedAccountBalanceSummaryQueryTests : InMemoryTestBase
{
    private readonly GetIncludedAccountBalanceSummaryQuery.Handler handler;

    public GetIncludedAccountBalanceSummaryQueryTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetSummary()
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
        result.Should().Be(80);
    }

    [Fact]
    public async Task DontIncludeDeactivatedAccounts()
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
        result.Should().Be(80);
    }
}
