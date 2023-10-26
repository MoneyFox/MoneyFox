namespace MoneyFox.Core.Tests.Features.PaymentByIdDeletions;

using Core.Common.Extensions;
using Core.Features.PaymentByIdDeletions;
using Domain.Tests.TestFramework;

public class DeletePaymentByIdHandler : InMemoryTestBase
{
    private readonly DeletePaymentById.Handler handler;

    public DeletePaymentByIdHandler()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task DontThrowExceptionWhenIdNotFound()
    {
        // Act
        await handler.Handle(command: new(12), cancellationToken: default);
    }

    [Fact]
    public async Task PaymentIsRemovedWhenAnEntryWithPassedIdExists()
    {
        // Arrange
        var testPayment = new TestData.ClearedExpense();
        Context.RegisterPayment(testPayment: testPayment);

        // Act
        await handler.Handle(command: new(testPayment.Id), cancellationToken: default);

        // Assert
        Context.Payments.Should().BeEmpty();
    }

    [Fact]
    public async Task PaymentIsRemoved_IncludingRecurringTransaction()
    {
        // Arrange
        var testRecurrence = new TestData.RecurringExpense();
        Context.RegisterRecurringTransaction(testRecurrence);
        var testPayment = new TestData.ClearedExpense { RecurringTransactionId = testRecurrence.RecurringTransactionId };
        Context.RegisterPayment(testPayment: testPayment);

        // Act
        await handler.Handle(command: new(PaymentId: testPayment.Id, DeleteRecurringPayment: true), cancellationToken: default);

        // Assert
        Context.Payments.Should().BeEmpty();
        var dbRecurringTransaction = Context.RecurringTransactions.Single();
        dbRecurringTransaction.EndDate.Should().Be(DateTime.Today.ToDateOnly());
    }
}
