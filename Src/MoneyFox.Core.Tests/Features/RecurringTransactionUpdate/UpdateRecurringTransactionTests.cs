namespace MoneyFox.Core.Tests.Features.RecurringTransactionUpdate;

using Core.Common.Extensions;
using Core.Features.RecurringTransactionUpdate;
using Domain;
using Domain.Aggregates;
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
        var newAmount = new Money(amount: 999, currency: Currencies.CHF);
        var categoryId = 200;
        var recurrence = Recurrence.Bimonthly;
        var endDate = DateTime.Today.AddDays(12).ToDateOnly();

        // Act
        var command = new UpdateRecurringTransaction.Command(
            RecurringTransactionId: testRecurringTransfer.RecurringTransactionId,
            UpdatedAmount: newAmount,
            UpdatedCategoryId: categoryId,
            UpdatedRecurrence: recurrence,
            UpdatedEndDate: endDate,
            IsLastDayOfMonth: true);

        await handler.Handle(request: command, cancellationToken: CancellationToken.None);

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
