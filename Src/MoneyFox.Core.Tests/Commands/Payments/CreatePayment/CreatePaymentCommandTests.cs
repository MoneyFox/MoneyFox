namespace MoneyFox.Core.Tests.Commands.Payments.CreatePayment;

using Core.Features._Legacy_.Payments.CreatePayment;
using Domain.Aggregates.AccountAggregate;
using FluentAssertions;

public class CreatePaymentCommandTests : InMemoryTestBase
{
    private readonly CreatePaymentCommand.Handler handler;

    public CreatePaymentCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task CreatePayment_PaymentSaved()
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        Context.Add(account);
        await Context.SaveChangesAsync();
        var payment = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: account);

        // Act
        await handler.Handle(request: new(payment), cancellationToken: default);

        // Assert
        Assert.Single(Context.Payments);
        (await Context.Payments.FindAsync(payment.Id)).Should().NotBeNull();
    }

    [Theory]
    [InlineData(PaymentType.Expense, 60)]
    [InlineData(PaymentType.Income, 100)]
    public async Task CreatePayment_AccountCurrentBalanceUpdated(PaymentType paymentType, decimal newCurrentBalance)
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        Context.Add(account);
        await Context.SaveChangesAsync();
        var payment = new Payment(date: DateTime.Now, amount: 20, type: paymentType, chargedAccount: account);

        // Act
        await handler.Handle(request: new(payment), cancellationToken: default);

        // Assert
        var loadedAccount = await Context.Accounts.FindAsync(account.Id);
        loadedAccount.Should().NotBeNull();
        loadedAccount.CurrentBalance.Should().Be(newCurrentBalance);
    }

    [Fact]
    public async Task CreatePaymentWithRecurring_PaymentSaved()
    {
        // Arrange
        var account = new Account(name: "test", initialBalance: 80);
        Context.Add(account);
        await Context.SaveChangesAsync();
        var payment = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: account);
        payment.AddRecurringPayment(recurrence: PaymentRecurrence.Monthly, isLastDayOfMonth: false);

        // Act
        await handler.Handle(request: new(payment), cancellationToken: default);

        // Assert
        Assert.Single(Context.Payments);
        Assert.Single(Context.RecurringPayments);
        (await Context.Payments.FindAsync(payment.Id)).Should().NotBeNull();
        (await Context.RecurringPayments.FindAsync(payment.RecurringPayment.Id)).Should().NotBeNull();
    }
}
