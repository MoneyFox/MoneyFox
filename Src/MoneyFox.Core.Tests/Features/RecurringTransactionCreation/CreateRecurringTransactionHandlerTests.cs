namespace MoneyFox.Core.Tests.Features.RecurringTransactionCreation;

using Core.Features.RecurringTransactionCreation;
using Domain.Tests.TestFramework;
using static Domain.Tests.TestFramework.RecurringTransactionAssertion;

public sealed class CreateRecurringTransactionHandlerTests : InMemoryTestBase
{
    private readonly CreateRecurringTransaction.Handler handler;

    public CreateRecurringTransactionHandlerTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task AddRecurringTransactionToDb()
    {
        // Arrange
        var testRecurringTransfer = new TestData.RecurringTransfer();

        // Act
        var command = new CreateRecurringTransaction.Command(
            recurringTransactionId: testRecurringTransfer.RecurringTransactionId,
            chargedAccount: testRecurringTransfer.ChargedAccount,
            targetAccount: testRecurringTransfer.TargetAccount,
            amount: testRecurringTransfer.Amount,
            categoryId: testRecurringTransfer.CategoryId,
            startDate: testRecurringTransfer.StartDate,
            endDate: testRecurringTransfer.EndDate,
            recurrence: testRecurringTransfer.Recurrence,
            note: testRecurringTransfer.Note,
            isLastDayOfMonth: testRecurringTransfer.IsLastDayOfMonth,
            lastRecurrence: testRecurringTransfer.LastRecurrence,
            isTransfer: testRecurringTransfer.IsTransfer);

        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbRecurringTransaction = Context.RecurringTransactions.Single();
        AssertRecurringTransaction(actual: dbRecurringTransaction, expected: testRecurringTransfer);
    }
}
