namespace MoneyFox.Ui.Tests.Views.Payments;

using Domain.Aggregates.AccountAggregate;
using FluentAssertions;
using Ui.Views.Payments.PaymentModification;
using Xunit;

public sealed class RecurrenceViewModelTests
{
    [Theory]
    [InlineData(PaymentRecurrence.Daily, false)]
    [InlineData(PaymentRecurrence.Weekly, false)]
    [InlineData(PaymentRecurrence.Biweekly, false)]
    [InlineData(PaymentRecurrence.Monthly, true)]
    [InlineData(PaymentRecurrence.Bimonthly, true)]
    [InlineData(PaymentRecurrence.Quarterly, true)]
    [InlineData(PaymentRecurrence.Biannually, true)]
    [InlineData(PaymentRecurrence.Yearly, true)]
    public void CheckAllowLastDayOfMonth(PaymentRecurrence recurrence, bool expectedAllowLastDayOfMonth)
    {
        new RecurrenceViewModel { Recurrence = recurrence }.AllowLastDayOfMonth.Should().Be(expectedAllowLastDayOfMonth);
    }

    [Theory]
    [InlineData(PaymentRecurrence.Daily)]
    [InlineData(PaymentRecurrence.Weekly)]
    [InlineData(PaymentRecurrence.Biweekly)]
    public void RemoveLastDayOfMonth_WhenRecurrenceIsChangedToInvalidSelection(PaymentRecurrence recurrence)
    {
        // Arrange
        var vm = new RecurrenceViewModel { Recurrence = PaymentRecurrence.Monthly, IsLastDayOfMonth = true };

        // Act
        vm.Recurrence = recurrence;

        // Assert
        vm.IsLastDayOfMonth.Should().BeFalse();
    }

    [Theory]
    [InlineData(PaymentRecurrence.Monthly)]
    [InlineData(PaymentRecurrence.Bimonthly)]
    [InlineData(PaymentRecurrence.Quarterly)]
    [InlineData(PaymentRecurrence.Biannually)]
    [InlineData(PaymentRecurrence.Yearly)]
    public void KeepLastDayOfMonth_WhenRecurrenceIsChangedToValidSelection(PaymentRecurrence recurrence)
    {
        // Arrange
        var vm = new RecurrenceViewModel { Recurrence = PaymentRecurrence.Monthly, IsLastDayOfMonth = true };

        // Act
        vm.Recurrence = recurrence;

        // Assert
        vm.IsLastDayOfMonth.Should().BeTrue();
    }
}
