namespace MoneyFox.Core.Tests.Features.RecurringTransactionUpdate;

using Core.Common.Extensions;
using Core.Features.RecurringTransactionUpdate;
using Domain;
using Domain.Aggregates;
using Domain.Aggregates.RecurringTransactionAggregate;
using Domain.Tests.TestFramework;
using static Domain.Tests.TestFramework.RecurringTransactionAssertion;

public sealed class UpdateRecurringTransactionHandlerTests : InMemoryTestBase
{
    private readonly UpdateRecurringTransaction.Handler handler;

    public UpdateRecurringTransactionHandlerTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task AddRecurringTransactionToDb()
    {
        // Arrange
        var testRecurringTransfer = new TestData.RecurringTransfer();
        Context.RegisterRecurringTransaction(testRecurringTransfer);
        var newAmount = new Money(amount: 999, currency: Currencies.CHF);
        var categoryId = 200;
        var recurrence = Recurrence.Bimonthly;
        var endDate = DateTime.Today.AddDays(12).ToDateOnly();

        // Act
        var command = new UpdateRecurringTransaction.Command(
            recurringTransactionId: testRecurringTransfer.RecurringTransactionId,
            updatedAmount: newAmount,
            updatedCategoryId: categoryId,
            updatedRecurrence: recurrence,
            updatedEndDate: endDate,
            isLastDayOfMonth: true);

        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbRecurringTransaction = Context.RecurringTransactions.Single();
        var expectedRecurringTransaction = testRecurringTransfer with
        {
            Amount = newAmount,
            CategoryId = categoryId,
            Recurrence = recurrence,
            EndDate = endDate,
            IsLastDayOfMonth = true
        };

        AssertRecurringTransaction(actual: dbRecurringTransaction, expected: expectedRecurringTransaction);
    }
}
