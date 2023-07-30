namespace MoneyFox.Domain.Tests.Aggregates.RecurringTransactionAggregate;

using Domain.Aggregates.RecurringTransactionAggregate;
using TestFramework;
using static TestFramework.RecurringTransactionAssertion;

public sealed class RecurringTransactionTests
{
    [Fact]
    public void CreatesAggregate()
    {
        // Arrange
        var testRecurringTransfer = new TestData.RecurringTransfer();

        // Act
        var recurringTransaction = RecurringTransaction.Create(
            recurringTransactionId: testRecurringTransfer.RecurringTransactionId,
            chargedAccount: testRecurringTransfer.ChargedAccount,
            targetAccount: testRecurringTransfer.TargetAccount,
            amount: testRecurringTransfer.Amount,
            categoryId: testRecurringTransfer.CategoryId,
            type: testRecurringTransfer.Type,
            startDate: testRecurringTransfer.StartDate,
            endDate: testRecurringTransfer.EndDate,
            recurrence: testRecurringTransfer.Recurrence,
            note: testRecurringTransfer.Note,
            isLastDayOfMonth: testRecurringTransfer.IsLastDayOfMonth,
            testRecurringTransfer.IsTransfer);

        // Assert
        AssertRecurringTransaction(actual: recurringTransaction, expected: testRecurringTransfer);
    }
}
