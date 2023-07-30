namespace MoneyFox.Core.Tests.Features;

using Core.Features.RecurringTransactionCreation;
using Domain.Tests.TestFramework;
using static Domain.Tests.TestFramework.RecurringTransactionAssertion;

public sealed class CreatePaymentTests : InMemoryTestBase
{
    private readonly CreateRecurringTransaction.Handler handler;

    public CreatePaymentTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task AddRecurringTransactionToDb()
    {
        // Arrange
        var testData = new TestData.RecurringTransfer();

        // Act
        var command = new CreateRecurringTransaction.Command(
            RecurringTransactionId: testData.RecurringTransactionId,
            ChargedAccount: testData.ChargedAccount,
            TargetAccount: testData.TargetAccount,
            Amount: testData.Amount,
            CategoryId: testData.CategoryId,
            Type: testData.Type,
            StartDate: testData.StartDate,
            EndDate: testData.EndDate,
            Recurrence: testData.Recurrence,
            Note: testData.Note,
            IsLastDayOfMonth: testData.IsLastDayOfMonth);

        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbRecurringTransaction = Context.RecurringTransactions.Single();
        AssertRecurringTransaction(actual: dbRecurringTransaction, expected: testData);
    }
}
