namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.RecurringTransactionAggregate;
using FluentAssertions;
using FluentAssertions.Execution;

internal static class RecurringTransactionAssertion
{
    public static void AssertRecurringTransaction(RecurringTransaction recurringTransaction,
        TestData.RecurringTransfer testRecurringTransfer)
    {
        using (new AssertionScope())
        {
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
}
