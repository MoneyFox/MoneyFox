namespace MoneyFox.Core.Tests.Commands.Payments.CreateRecurringPayments;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Commands.Payments.CreateRecurringPayments;
using Core.Common.Extensions;
using FluentAssertions;
using Infrastructure.Persistence;

public class CreateRecurringPaymentsCommandTests : InMemoryTestBase
{
    private readonly CreateRecurringPaymentsCommand.Handler handler;

    public CreateRecurringPaymentsCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task PaymentsClearedAndSaved()
    {
        // Arrange
        var payment = new Payment(date: DateTime.Now.AddDays(-1), amount: 166, type: PaymentType.Expense, chargedAccount: new("Foo"));
        payment.AddRecurringPayment(recurrence: PaymentRecurrence.Daily, isLastDayOfMonth: false);
        Context.AddRange(payment);
        await Context.SaveChangesAsync();

        // Act
        await handler.Handle(request: new(), cancellationToken: default);
        var loadedPayments = Context.Payments.ToList();

        // Assert
        loadedPayments.Should().HaveCount(2);
        loadedPayments.ForEach(x => x.Amount.Should().Be(166));
    }

    [Theory]
    [InlineData(PaymentRecurrence.Monthly, 1, true)]
    [InlineData(PaymentRecurrence.Monthly, 1, false)]
    [InlineData(PaymentRecurrence.Bimonthly, 2, true)]
    [InlineData(PaymentRecurrence.Bimonthly, 2, false)]
    [InlineData(PaymentRecurrence.Quarterly, 3, true)]
    [InlineData(PaymentRecurrence.Quarterly, 3, false)]
    [InlineData(PaymentRecurrence.Biannually, 6, true)]
    [InlineData(PaymentRecurrence.Biannually, 6, false)]
    [InlineData(PaymentRecurrence.Yearly, 12, true)]
    [InlineData(PaymentRecurrence.Yearly, 12, false)]
    public async Task LastDayOfMonthValidation(PaymentRecurrence recurrence, int paymentDateMonthsBack, bool isLastDayOfMonth)
    {
        // Arrange
        var payment = new Payment(
            date: DateTime.Today.AddMonths(-paymentDateMonthsBack).GetFirstDayOfMonth(),
            amount: 333,
            type: PaymentType.Expense,
            chargedAccount: new("Foo"));

        payment.AddRecurringPayment(recurrence: recurrence, isLastDayOfMonth: isLastDayOfMonth);
        Context.AddRange(payment);
        await Context.SaveChangesAsync();

        // Act
        await handler.Handle(request: new(), cancellationToken: default);
        var loadedPayments = Context.Payments.ToList();

        // Assert
        loadedPayments.Should().HaveCount(2);
        loadedPayments.ForEach(x => x.Amount.Should().Be(333));
        loadedPayments[1].Date.Should().Be(isLastDayOfMonth ? DateTime.Today.GetLastDayOfMonth() : DateTime.Today.GetFirstDayOfMonth());
    }
}
