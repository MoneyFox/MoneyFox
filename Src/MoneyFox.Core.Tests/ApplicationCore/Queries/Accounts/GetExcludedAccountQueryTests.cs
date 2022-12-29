namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;
using TestFramework;

[ExcludeFromCodeCoverage]
public class GetExcludedAccountQueryTests
{
    private readonly AppDbContext context;
    private readonly GetExcludedAccountQuery.Handler handler;

    public GetExcludedAccountQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task GetExcludedAccountQuery_CorrectNumberLoaded()
    {
        // Arrange
        var accountExcluded = new Account(name: "test", initialBalance: 80, isExcluded: true);
        var accountIncluded = new Account(name: "test", initialBalance: 80);
        await context.AddAsync(accountExcluded);
        await context.AddAsync(accountIncluded);
        await context.SaveChangesAsync();

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
        await context.AddAsync(accountExcluded);
        await context.AddAsync(accountIncluded);
        await context.AddAsync(accountDeactivated);
        await context.SaveChangesAsync();

        // Act
        var resultList = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        resultList.Should().ContainSingle();
        resultList[0].CurrentBalance.Should().Be(80);
    }
}
