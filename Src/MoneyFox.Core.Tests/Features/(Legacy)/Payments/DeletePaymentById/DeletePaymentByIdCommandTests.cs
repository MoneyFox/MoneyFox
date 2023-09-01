namespace MoneyFox.Core.Tests.Features._Legacy_.Payments.DeletePaymentById;

using Core.Features._Legacy_.Payments.DeletePaymentById;
using Domain.Aggregates.AccountAggregate;
using Domain.Tests.TestFramework;

public class DeletePaymentByIdCommandTests : InMemoryTestBase
{
    private readonly DeletePaymentById.Handler handler;

    public DeletePaymentByIdCommandTests()
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
        Assert.Empty(Context.Payments);
    }
}
