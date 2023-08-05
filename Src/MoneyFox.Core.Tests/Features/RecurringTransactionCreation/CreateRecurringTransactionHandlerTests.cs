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
            RecurringTransactionId: testRecurringTransfer.RecurringTransactionId,
            ChargedAccount: testRecurringTransfer.ChargedAccount,
            TargetAccount: testRecurringTransfer.TargetAccount,
            Amount: testRecurringTransfer.Amount,
            CategoryId: testRecurringTransfer.CategoryId,
            StartDate: testRecurringTransfer.StartDate,
            EndDate: testRecurringTransfer.EndDate,
            Recurrence: testRecurringTransfer.Recurrence,
            Note: testRecurringTransfer.Note,
            IsLastDayOfMonth: testRecurringTransfer.IsLastDayOfMonth,
            IsTransfer: testRecurringTransfer.IsTransfer);

        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbRecurringTransaction = Context.RecurringTransactions.Single();
        AssertRecurringTransaction(actual: dbRecurringTransaction, expected: testRecurringTransfer);
    }
}
