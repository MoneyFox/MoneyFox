namespace MoneyFox.Core.Tests.Commands.Payments.CreatePayment;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Commands.Payments.CreatePayment;
using FluentAssertions;
using Infrastructure.Persistence;


public class CreatePaymentCommandTests
{
    private readonly AppDbContext context;

    private readonly CreatePaymentCommand.Handler handler;

    public CreatePaymentCommandTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task CreatePayment_PaymentSaved()
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        context.Add(account);
        await context.SaveChangesAsync();
        var payment = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: account);

        // Act
        await handler.Handle(request: new(payment), cancellationToken: default);

        // Assert
        Assert.Single(context.Payments);
        (await context.Payments.FindAsync(payment.Id)).Should().NotBeNull();
    }

    [Theory]
    [InlineData(PaymentType.Expense, 60)]
    [InlineData(PaymentType.Income, 100)]
    public async Task CreatePayment_AccountCurrentBalanceUpdated(PaymentType paymentType, decimal newCurrentBalance)
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        context.Add(account);
        await context.SaveChangesAsync();
        var payment = new Payment(date: DateTime.Now, amount: 20, type: paymentType, chargedAccount: account);

        // Act
        await handler.Handle(request: new(payment), cancellationToken: default);

        // Assert
        var loadedAccount = await context.Accounts.FindAsync(account.Id);
        loadedAccount.Should().NotBeNull();
        loadedAccount.CurrentBalance.Should().Be(newCurrentBalance);
    }

    [Fact]
    public async Task CreatePaymentWithRecurring_PaymentSaved()
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        context.Add(account);
        await context.SaveChangesAsync();
        var payment = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: account);
        payment.AddRecurringPayment(recurrence: PaymentRecurrence.Monthly, isLastDayOfMonth: false);

        // Act
        await handler.Handle(request: new(payment), cancellationToken: default);

        // Assert
        Assert.Single(context.Payments);
        Assert.Single(context.RecurringPayments);
        (await context.Payments.FindAsync(payment.Id)).Should().NotBeNull();
        (await context.RecurringPayments.FindAsync(payment.RecurringPayment.Id)).Should().NotBeNull();
    }
}
