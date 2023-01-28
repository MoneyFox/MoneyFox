namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts.GetAccountEndOfMonthBalance;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using Core.Common.Helpers;
using FluentAssertions;
using Infrastructure.Persistence;
using NSubstitute;

[ExcludeFromCodeCoverage]
public class GetAccountEndOfMonthBalanceQueryTests
{
    private readonly AppDbContext context;
    private readonly GetAccountEndOfMonthBalanceQuery.Handler handler;
    private readonly ISystemDateHelper systemDateHelper;

    public GetAccountEndOfMonthBalanceQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        systemDateHelper = Substitute.For<ISystemDateHelper>();
        handler = new(appDbContext: context, systemDateHelper: systemDateHelper);
    }

    [Fact]
    public async Task GetCorrectSumForSingleAccount()
    {
        // Arrange
        systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
        var account1 = new Account(name: "test", initialBalance: 100);
        var account2 = new Account(name: "test", initialBalance: 200);
        var payment1 = new Payment(date: new(year: 2020, month: 09, day: 15), amount: 50, type: PaymentType.Income, chargedAccount: account1);
        var payment2 = new Payment(date: new(year: 2020, month: 09, day: 25), amount: 50, type: PaymentType.Expense, chargedAccount: account2);
        await context.AddAsync(account1);
        await context.AddAsync(account2);
        await context.AddAsync(payment1);
        await context.AddAsync(payment2);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(account1.Id), cancellationToken: default);

        // Assert
        result.Should().Be(150);
    }

    [Fact]
    public async Task GetCorrectSumForWithDeactivatedAccount()
    {
        // Arrange
        systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
        var account1 = new Account(name: "test", initialBalance: 100);
        var account2 = new Account(name: "test", initialBalance: 200);
        var account3 = new Account(name: "test", initialBalance: 200);
        var payment1 = new Payment(date: new(year: 2020, month: 09, day: 15), amount: 50, type: PaymentType.Income, chargedAccount: account1);
        var payment2 = new Payment(date: new(year: 2020, month: 09, day: 25), amount: 50, type: PaymentType.Expense, chargedAccount: account2);
        account3.Deactivate();
        await context.AddAsync(account1);
        await context.AddAsync(account2);
        await context.AddAsync(account3);
        await context.AddAsync(payment1);
        await context.AddAsync(payment2);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(account1.Id), cancellationToken: default);

        // Assert
        result.Should().Be(150);
    }
}
