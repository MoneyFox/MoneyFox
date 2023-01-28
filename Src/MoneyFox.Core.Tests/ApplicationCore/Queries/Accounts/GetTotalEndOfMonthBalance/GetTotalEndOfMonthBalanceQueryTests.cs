namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts.GetTotalEndOfMonthBalance;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Queries;
using Core.Common.Helpers;
using FluentAssertions;
using Infrastructure.Persistence;
using NSubstitute;

public class GetTotalEndOfMonthBalanceQueryTests
{
    private readonly AppDbContext context;
    private readonly GetTotalEndOfMonthBalanceQuery.Handler handler;
    private readonly ISystemDateHelper systemDateHelper;

    public GetTotalEndOfMonthBalanceQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        systemDateHelper = Substitute.For<ISystemDateHelper>();
        handler = new(appDbContext: context, systemDateHelper: systemDateHelper);
    }

    [Fact]
    public async Task GetIncludedAccountBalanceSummary_CorrectSum()
    {
        // Arrange
        systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
        var accountIncluded = new Account(name: "test", initialBalance: 100);
        var payment = new Payment(date: new(year: 2020, month: 09, day: 25), amount: 50, type: PaymentType.Expense, chargedAccount: accountIncluded);
        await context.AddAsync(accountIncluded);
        await context.AddAsync(payment);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        result.Should().Be(50);
    }

    [Fact]
    public async Task DontIncludeDeactivatedAccountsInBalance()
    {
        // Arrange
        systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
        var accountIncluded = new Account(name: "test", initialBalance: 100);
        var accountDeactivated = new Account(name: "test", initialBalance: 100);
        accountDeactivated.Deactivate();
        var payment = new Payment(date: new(year: 2020, month: 09, day: 25), amount: 50, type: PaymentType.Expense, chargedAccount: accountIncluded);
        await context.AddAsync(accountIncluded);
        await context.AddAsync(accountDeactivated);
        await context.AddAsync(payment);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        result.Should().Be(50);
    }
}
