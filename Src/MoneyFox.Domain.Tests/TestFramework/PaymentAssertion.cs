namespace MoneyFox.Domain.Tests.TestFramework;

using Domain.Aggregates.AccountAggregate;
using FluentAssertions.Execution;

internal static class PaymentAssertion
{
    public static void AssertPayment(Payment actual, TestData.IPayment expected)
    {
        using (new AssertionScope())
        {
            actual.RecurringTransactionId.Should().Be(expected.RecurringTransactionId);
            actual.ChargedAccount.Id.Should().Be(expected.ChargedAccount.Id);
            actual.TargetAccount?.Id.Should().Be(expected.TargetAccount?.Id);
            actual.Amount.Should().Be(expected.Amount);
            actual.Type.Should().Be(expected.Type);
            actual.Category?.Id.Should().Be(expected.Category?.Id);
            actual.Date.Should().Be(expected.Date);
            actual.Note.Should().Be(expected.Note);
        }
    }
}
