namespace MoneyFox.Core.Tests.Features.RecurringTransactionCreation;

using Core.Common.Extensions;
using Core.Features.RecurringTransactionCreation;
using Domain.Exceptions;
using Domain.Tests.TestFramework;
using FluentAssertions;

public sealed class CreateRecurringTransactionCommandTests
{
    [Fact]
    public void ShouldThrowException_WhenEndDateIsInPast()
    {
        // Arrange
        var testRecurringTransfer = new TestData.RecurringTransfer();

        // Act
        var act = () => new CreateRecurringTransaction.Command(
            recurringTransactionId: testRecurringTransfer.RecurringTransactionId,
            chargedAccount: testRecurringTransfer.ChargedAccount,
            targetAccount: testRecurringTransfer.TargetAccount,
            amount: testRecurringTransfer.Amount,
            categoryId: testRecurringTransfer.CategoryId,
            startDate: testRecurringTransfer.StartDate,
            endDate: DateTime.Today.AddDays(-1).ToDateOnly(),
            recurrence: testRecurringTransfer.Recurrence,
            note: testRecurringTransfer.Note,
            isLastDayOfMonth: testRecurringTransfer.IsLastDayOfMonth,
            isTransfer: testRecurringTransfer.IsTransfer);

        // Assert
        act.Should().Throw<InvalidEndDateException>();
    }
}
