namespace MoneyFox.Core.Tests.Features.RecurringTransactionUpdate;

using Core.Features.RecurringTransactionUpdate;
using Domain.Tests.TestFramework;
using static Domain.Tests.TestFramework.RecurringTransactionAssertion;

public sealed class UpdateRecurringTransactionTests : InMemoryTestBase
{
    private readonly UpdateRecurringTransaction.Handler handler;

    public UpdateRecurringTransactionTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task AddRecurringTransactionToDb()
    {
        // Arrange
        var testRecurringTransfer = new TestData.RecurringTransfer();
        Context.RegisterRecurringTransaction(testRecurringTransfer);

        // Act
        var command = new UpdateRecurringTransaction.Command(RecurringTransactionId: testRecurringTransfer.RecurringTransactionId);
        await handler.Handle(request: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbRecurringTransaction = Context.RecurringTransactions.Single();
        AssertRecurringTransaction(actual: dbRecurringTransaction, expected: testRecurringTransfer);
    }
}
