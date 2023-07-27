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
        AssertRecurringTransaction(recurringTransaction: recurringTransaction, testRecurringTransfer: testRecurringTransfer);
    }
}
