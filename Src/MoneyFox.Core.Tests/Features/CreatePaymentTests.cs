namespace MoneyFox.Core.Tests.Features;

using AutoMapper.Configuration.Annotations;
using Core.Features.PaymentCreation;
using Domain.Tests.TestFramework;
using static Domain.Tests.TestFramework.RecurringTransactionAssertion;

public sealed class CreatePaymentTests : InMemoryTestBase
{
    private readonly CreatePayment.Handler handler;

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
        var command = new CreatePayment.Command(
            testData.RecurringTransactionId,
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
