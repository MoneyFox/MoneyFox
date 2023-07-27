namespace MoneyFox.Domain.Tests.Aggregates.RecurringTransactionAggregate;

using Domain.Aggregates.RecurringTransactionAggregate;
using FluentAssertions;
using TestFramework;

public sealed class RecurringTransactionTests
{
    [Fact]
    public void CreatesAggregate()
    {
        // Arrange
        var testRecurringTransfer = new TestData.RecurringTransfer();

        // Act
        var recurringTransaction = RecurringTransaction.Create(
            id: testRecurringTransfer.Id,
            startDate: testRecurringTransfer.StartDate,
            endDate: testRecurringTransfer.EndDate,
            amount: testRecurringTransfer.Amount,
            type: testRecurringTransfer.Type,
            note: testRecurringTransfer.Note,
            chargedAccount: testRecurringTransfer.ChargedAccount,
            targetAccount: testRecurringTransfer.TargetAccount,
            categoryId: testRecurringTransfer.CategoryId,
            recurrence: testRecurringTransfer.Recurrence,
            isLastDayOfMonth: testRecurringTransfer.IsLastDayOfMonth);

        // Assert
        recurringTransaction.Id.Should().Be(testRecurringTransfer.Id);
        recurringTransaction.StartDate.Should().Be(testRecurringTransfer.StartDate);
        recurringTransaction.EndDate.Should().Be(testRecurringTransfer.EndDate);
        recurringTransaction.Amount.Should().Be(testRecurringTransfer.Amount);
        recurringTransaction.Type.Should().Be(testRecurringTransfer.Type);
        recurringTransaction.Note.Should().Be(testRecurringTransfer.Note);
        recurringTransaction.ChargedAccount.Should().Be(testRecurringTransfer.ChargedAccount);
        recurringTransaction.TargetAccount.Should().Be(testRecurringTransfer.TargetAccount);
        recurringTransaction.CategoryId.Should().Be(testRecurringTransfer.CategoryId);
        recurringTransaction.Recurrence.Should().Be(testRecurringTransfer.Recurrence);
        recurringTransaction.IsLastDayOfMonth.Should().Be(testRecurringTransfer.IsLastDayOfMonth);
        recurringTransaction.LastRecurrence.Should().Be(DateOnly.FromDateTime(DateTime.Today));
    }
}
