namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Payments.GetPaymentsForAccountId;

using Core.ApplicationCore.Queries.GetPaymentsForAccountIdQuery;
using FluentAssertions;
using MoneyFox.Domain.Aggregates.AccountAggregate;

public class GetPaymentsForAccountIdQueryTests : InMemoryTestBase
{
    private readonly GetPaymentsForAccountIdQuery.Handler handler;

    public GetPaymentsForAccountIdQueryTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetPaymentsForAccountId_CorrectAccountId_PaymentFound()
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: account);
        var payment2 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new(name: "test", initialBalance: 80));
        await Context.AddAsync(payment1);
        await Context.AddAsync(payment2);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(accountId: account.Id, timeRangeStart: DateTime.Now.AddDays(-1), timeRangeEnd: DateTime.Now.AddDays(1)),
            cancellationToken: default);

        // Assert
        result.First().Id.Should().Be(payment1.Id);
    }

    [Fact]
    public async Task GetPaymentsForAccountId_CorrectDateRange_PaymentFound()
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        var payment1 = new Payment(date: DateTime.Now.AddDays(-2), amount: 20, type: PaymentType.Expense, chargedAccount: account);
        var payment2 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: account);
        await Context.AddAsync(payment1);
        await Context.AddAsync(payment2);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(accountId: account.Id, timeRangeStart: DateTime.Now.AddDays(-1), timeRangeEnd: DateTime.Now.AddDays(1)),
            cancellationToken: default);

        // Assert
        result.First().Id.Should().Be(payment2.Id);
    }

    [Theory]
    [InlineData(PaymentTypeFilter.All)]
    [InlineData(PaymentTypeFilter.Expense)]
    [InlineData(PaymentTypeFilter.Income)]
    [InlineData(PaymentTypeFilter.Transfer)]
    public async Task GetPaymentsForAccountId_CorrectPaymentType_PaymentFound(PaymentTypeFilter filteredPaymentType)
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        var accountxfer = new Account(name: "dest", initialBalance: 80);
        var payment1 = new Payment(date: DateTime.Now, amount: 10, type: PaymentType.Expense, chargedAccount: account);
        var payment2 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Income, chargedAccount: account);
        var payment3 = new Payment(
            date: DateTime.Now,
            amount: 30,
            type: PaymentType.Transfer,
            chargedAccount: account,
            targetAccount: accountxfer);

        await Context.AddAsync(payment1);
        await Context.AddAsync(payment2);
        await Context.AddAsync(payment3);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(
            request: new(
                accountId: account.Id,
                timeRangeStart: DateTime.Now.AddDays(-1),
                timeRangeEnd: DateTime.Now.AddDays(1),
                isClearedFilterActive: default,
                isRecurringFilterActive: default,
                filteredPaymentType: filteredPaymentType),
            cancellationToken: default);

        var expectedamount = filteredPaymentType switch
        {
            PaymentTypeFilter.Expense => 10,
            PaymentTypeFilter.Income => 20,
            PaymentTypeFilter.Transfer => 30,
            _ => 0
        };

        // Assert
        Assert.Equal(expected: result.Count, actual: filteredPaymentType == PaymentTypeFilter.All ? 3 : 1);
        if (filteredPaymentType != PaymentTypeFilter.All)
        {
            result.First().Amount.Should().Be(expectedamount);
        }
    }
}
