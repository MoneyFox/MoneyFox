namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using Infrastructure.Persistence;

public class GetIncludedAccountQueryTests
{
    private readonly AppDbContext context;
    private readonly GetIncludedAccountQuery.Handler handler;

    public GetIncludedAccountQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task GetIncludedAccountQuery_CorrectNumberLoaded()
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
        await context.AddAsync(accountExcluded);
        await context.AddAsync(accountIncluded);
        await context.AddAsync(accountDeactivated);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        Assert.Single(result);
    }
}
