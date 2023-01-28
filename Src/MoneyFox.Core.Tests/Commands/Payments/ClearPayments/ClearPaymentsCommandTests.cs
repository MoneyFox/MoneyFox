namespace MoneyFox.Core.Tests.Commands.Payments.ClearPayments;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Commands.Payments.ClearPayments;
using FluentAssertions;
using Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class ClearPaymentsCommandTests
{
    private readonly AppDbContext context;
    private readonly ClearPaymentsCommand.Handler handler;

    public ClearPaymentsCommandTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
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

        context.AddRange(paymentList);
        await context.SaveChangesAsync();

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

        context.AddRange(paymentList);
        await context.SaveChangesAsync();

        // Act
        var command = new ClearPaymentsCommand();
        await handler.Handle(request: command, cancellationToken: default);
        var loadedPayments = context.Payments.ToList();

        // Assert
        loadedPayments[0].IsCleared.Should().BeFalse();
        loadedPayments[1].IsCleared.Should().BeTrue();
        loadedPayments[2].IsCleared.Should().BeTrue();
    }
}
