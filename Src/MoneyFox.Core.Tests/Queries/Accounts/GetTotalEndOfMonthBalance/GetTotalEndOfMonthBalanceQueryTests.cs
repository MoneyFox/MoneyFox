namespace MoneyFox.Core.Tests.Queries.Accounts.GetTotalEndOfMonthBalance;

using Core.Common;
using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using FluentAssertions;
using NSubstitute;

public class GetTotalEndOfMonthBalanceQueryTests : InMemoryTestBase
{
    private readonly GetTotalEndOfMonthBalanceQuery.Handler handler;
    private readonly ISystemDateHelper systemDateHelper;

    public GetTotalEndOfMonthBalanceQueryTests()
    {
        systemDateHelper = Substitute.For<ISystemDateHelper>();
        handler = new(appDbContext: Context, systemDateHelper: systemDateHelper);
    }

    [Fact]
    public async Task GetIncludedAccountBalanceSummary_CorrectSum()
    {
        // Arrange
        systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
        var accountIncluded = new Account(name: "test", initialBalance: 100);
        var payment = new Payment(date: new(year: 2020, month: 09, day: 25), amount: 50, type: PaymentType.Expense, chargedAccount: accountIncluded);
        await Context.AddAsync(accountIncluded);
        await Context.AddAsync(payment);
        await Context.SaveChangesAsync();

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
        await Context.AddAsync(accountIncluded);
        await Context.AddAsync(accountDeactivated);
        await Context.AddAsync(payment);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(), cancellationToken: default);

        // Assert
        result.Should().Be(50);
    }
}
