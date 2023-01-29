namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;

public class GetIncludedAccountBalanceSummaryTests : InMemoryTestBase
{
    private readonly GetIncludedAccountBalanceSummaryQuery.Handler handler;

    public GetIncludedAccountBalanceSummaryTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetIncludedAccountBalanceSummary_CorrectSum()
    {
        // Arrange
        var accountExcluded = new Account(name: "test", initialBalance: 80, isExcluded: true);
        var accountIncluded1 = new Account(name: "test", initialBalance: 100);
        var accountIncluded2 = new Account(name: "test", initialBalance: 120);
        await Context.AddAsync(accountExcluded);
        await Context.AddAsync(accountIncluded1);
        await Context.AddAsync(accountIncluded2);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        result.Should().Be(220);
    }
}
