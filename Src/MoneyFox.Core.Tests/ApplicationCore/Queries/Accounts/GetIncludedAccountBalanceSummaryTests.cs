namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;
using TestFramework;

[ExcludeFromCodeCoverage]
public class GetIncludedAccountBalanceSummaryTests
{
    private readonly AppDbContext context;
    private readonly GetIncludedAccountBalanceSummaryQuery.Handler handler;

    public GetIncludedAccountBalanceSummaryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task GetIncludedAccountBalanceSummary_CorrectSum()
    {
        // Arrange
        var accountExcluded = new Account(name: "test", initialBalance: 80, isExcluded: true);
        var accountIncluded1 = new Account(name: "test", initialBalance: 100);
        var accountIncluded2 = new Account(name: "test", initialBalance: 120);
        await context.AddAsync(accountExcluded);
        await context.AddAsync(accountIncluded1);
        await context.AddAsync(accountIncluded2);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        result.Should().Be(220);
    }
}
