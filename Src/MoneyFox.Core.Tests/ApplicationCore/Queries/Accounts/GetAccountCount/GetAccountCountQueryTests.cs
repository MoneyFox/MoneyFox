namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts.GetAccountCount;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class GetAccountCountQueryTests : InMemoryTestBase
{
    private readonly GetAccountCountQuery.Handler handler;

    public GetAccountCountQueryTests()
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
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public async Task HandleDeactivatedAccountsCorrectly()
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        var accountDeactivated = new Account(name: "test", initialBalance: 80);
        accountDeactivated.Deactivate();
        await Context.AddAsync(accountDeactivated);
        await Context.AddAsync(account);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        result.Should().Be(1);
    }
}
