namespace MoneyFox.Core.Tests.Features.RecurringTransactionUpdate;

using Core.Features.RecurringTransactionUpdate;
using Domain;
using Domain.Aggregates.RecurringTransactionAggregate;
using Domain.Exceptions;
using FluentAssertions;

public sealed class UpdateRecurringTransactionCommandTests
{
    [Fact]
    public void ShouldThrowException_WhenEndDateIsInPast()
    {
        // Act
        var act = () => new UpdateRecurringTransaction.Command(
            recurringTransactionId: Guid.Empty,
            updatedAmount: Money.Zero(Currencies.CHF),
            updatedCategoryId: null,
            updatedRecurrence: Recurrence.Monthly,
            updatedEndDate: DateOnly.FromDateTime(DateTime.Today).AddDays(-1),
            isLastDayOfMonth: false);

        // Assert
        act.Should().Throw<InvalidEndDateException>();
    }

    [Fact]
    public void AssignValuesCorrectly()
    {
        // Arrange
        var recurringTransactionId = Guid.NewGuid();
        var updatedAmount = new Money(amount: 99, currency: Currencies.CHF);
        var updatedEndDate = DateOnly.FromDateTime(DateTime.Today);
        var updatedCategoryId = 1;
        var updatedRecurrence = Recurrence.Monthly;

        // Act
        var command = new UpdateRecurringTransaction.Command(
            recurringTransactionId: recurringTransactionId,
            updatedAmount: updatedAmount,
            updatedCategoryId: updatedCategoryId,
            updatedRecurrence: updatedRecurrence,
            updatedEndDate: updatedEndDate,
            isLastDayOfMonth: true);

        // Assert
        command.RecurringTransactionId.Should().Be(recurringTransactionId);
        command.UpdatedAmount.Should().Be(updatedAmount);
        command.UpdatedCategoryId.Should().Be(updatedCategoryId);
        command.UpdatedRecurrence.Should().Be(updatedRecurrence);
        command.UpdatedEndDate.Should().Be(updatedEndDate);
    }
}
