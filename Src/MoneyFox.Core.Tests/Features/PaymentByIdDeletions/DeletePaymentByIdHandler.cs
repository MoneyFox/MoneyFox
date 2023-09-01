namespace MoneyFox.Core.Tests.Features.PaymentByIdDeletions;

using MoneyFox.Core.Features.PaymentByIdDeletions;
using MoneyFox.Domain.Tests.TestFramework;

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
        Assert.Empty(Context.Payments);
    }
}
