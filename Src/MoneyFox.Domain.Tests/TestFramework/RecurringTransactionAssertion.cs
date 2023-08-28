namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.RecurringTransactionAggregate;
using FluentAssertions.Execution;

internal static class RecurringTransactionAssertion
{
    public static void AssertRecurringTransaction(RecurringTransaction actual, TestData.RecurringTransfer expected)
    {
        using (new AssertionScope())
        {
            actual.Id.Should().NotBeNull();
            actual.RecurringTransactionId.Should().Be(expected.RecurringTransactionId);
            actual.ChargedAccountId.Should().Be(expected.ChargedAccount);
            actual.TargetAccountId.Should().Be(expected.TargetAccount);
            actual.Amount.Should().Be(expected.Amount);
            actual.CategoryId.Should().Be(expected.CategoryId);
            actual.StartDate.Should().Be(expected.StartDate);
            actual.EndDate.Should().Be(expected.EndDate);
            actual.Recurrence.Should().Be(expected.Recurrence);
            actual.Note.Should().Be(expected.Note);
            actual.IsLastDayOfMonth.Should().Be(expected.IsLastDayOfMonth);
            actual.LastRecurrence.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        }
    }
}
