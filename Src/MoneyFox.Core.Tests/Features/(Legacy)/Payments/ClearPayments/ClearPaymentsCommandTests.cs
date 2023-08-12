namespace MoneyFox.Core.Tests.Features._Legacy_.Payments.ClearPayments;

using Core.Features._Legacy_.Payments.ClearPayments;
using Domain.Aggregates.AccountAggregate;
using FluentAssertions;

public class ClearPaymentsCommandTests : InMemoryTestBase
{
    private readonly ClearPaymentsCommand.Handler handler;

    public ClearPaymentsCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task PaymentsClearedCorrectly()
    {
        // Arrange
        var paymentList = new List<Payment>
        {
            new(date: DateTime.Now.AddDays(1), amount: 100, type: PaymentType.Expense, chargedAccount: new("Foo")),
            new(date: DateTime.Now, amount: 100, type: PaymentType.Expense, chargedAccount: new("Foo")),
            new(date: DateTime.Now.AddDays(-1), amount: 100, type: PaymentType.Expense, chargedAccount: new("Foo"))
        };

        Context.AddRange(paymentList);
        await Context.SaveChangesAsync();

        // Act
        var command = new ClearPaymentsCommand();
        await handler.Handle(request: command, cancellationToken: default);

        // Assert
        paymentList[0].IsCleared.Should().BeFalse();
        paymentList[1].IsCleared.Should().BeTrue();
        paymentList[2].IsCleared.Should().BeTrue();
    }

    [Fact]
    public async Task PaymentsClearedAndSaved()
    {
        // Arrange
        var paymentList = new List<Payment>
        {
            new(date: DateTime.Now.AddDays(1), amount: 100, type: PaymentType.Expense, chargedAccount: new("Foo")),
            new(date: DateTime.Now, amount: 100, type: PaymentType.Expense, chargedAccount: new("Foo")),
            new(date: DateTime.Now.AddDays(-1), amount: 100, type: PaymentType.Expense, chargedAccount: new("Foo"))
        };

        Context.AddRange(paymentList);
        await Context.SaveChangesAsync();

        // Act
        var command = new ClearPaymentsCommand();
        await handler.Handle(request: command, cancellationToken: default);
        var loadedPayments = Context.Payments.ToList();

        // Assert
        loadedPayments[0].IsCleared.Should().BeFalse();
        loadedPayments[1].IsCleared.Should().BeTrue();
        loadedPayments[2].IsCleared.Should().BeTrue();
    }
}
