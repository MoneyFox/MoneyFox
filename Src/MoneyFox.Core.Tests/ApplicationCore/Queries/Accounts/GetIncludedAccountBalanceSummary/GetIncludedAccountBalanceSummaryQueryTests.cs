namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts.GetIncludedAccountBalanceSummary;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;
using TestFramework;

[ExcludeFromCodeCoverage]
public class GetIncludedAccountBalanceSummaryQueryTests
{
    private readonly AppDbContext context;
    private readonly GetIncludedAccountBalanceSummaryQuery.Handler handler;

    public GetIncludedAccountBalanceSummaryQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task GetSummary()
    {
        // Arrange
        var accountExcluded = new Account(name: "test", initialBalance: 80, isExcluded: true);
        var accountIncluded = new Account(name: "test", initialBalance: 80);
        await context.AddAsync(accountExcluded);
        await context.AddAsync(accountIncluded);
        await context.SaveChangesAsync();

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
        await context.AddAsync(accountExcluded);
        await context.AddAsync(accountIncluded);
        await context.AddAsync(accountDeactivated);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        result.Should().Be(80);
    }
}
