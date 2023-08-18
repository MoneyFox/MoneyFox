namespace MoneyFox.Core.Tests.Features._Legacy_.Payments.CreatePayment;

using Aptabase.Maui;
using Core.Features._Legacy_.Payments.CreatePayment;
using Domain.Aggregates.AccountAggregate;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

public class CreatePaymentCommandTests : InMemoryTestBase
{
    private readonly CreatePaymentCommand.Handler handler;

    public CreatePaymentCommandTests()
    {
        handler = new(appDbContext: Context, aptabaseClient: Substitute.For<IAptabaseClient>());
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
        var loadedAccount = await Context.Accounts.SingleAsync(a => a.Id == account.Id);
        loadedAccount.Should().NotBeNull();
        loadedAccount.CurrentBalance.Should().Be(newCurrentBalance);
    }
}
