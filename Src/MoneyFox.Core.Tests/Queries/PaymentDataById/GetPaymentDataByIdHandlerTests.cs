namespace MoneyFox.Core.Tests.Queries.PaymentDataById;

using Core.Queries.PaymentDataById;
using Domain.Tests.TestFramework;
using FluentAssertions;

public class GetPaymentDataByIdHandlerTests : InMemoryTestBase
{
    private readonly GetPaymentDataById.Handler handler;

    protected GetPaymentDataByIdHandlerTests()
    {
        handler = new(Context);
    }

    public sealed class PaymentNotFound : GetPaymentDataByIdHandlerTests
    {
        [Fact]
        public async Task ShouldThrowException()
        {
            // Act / Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(query: new(999), cancellationToken: default));
        }
    }

    public sealed class OnTimePayment : GetPaymentDataByIdHandlerTests
    {
        [Fact]
        public async Task ReturnsCorrectData()
        {
            // Arrange
            var payment = new TestData.ClearedExpense();
            Context.RegisterPayment(payment);

            // Act
            var result = await handler.Handle(query: new(payment.Id), cancellationToken: default);

            // Assert
            result.PaymentId.Should().Be(payment.Id);
            result.Amount.Should().Be(payment.Amount);
            result.ChargedAccountId.Should().Be(payment.ChargedAccount.Id);
            result.TargetAccountId.Should().Be(payment.TargetAccount?.Id);
            result.Date.Should().Be(payment.Date);
            result.IsCleared.Should().Be(payment.IsCleared);
            result.Type.Should().Be(payment.Type);
            result.Note.Should().Be(payment.Note);
            result.Created.Should().BeAfter(DateTime.Today);
            result.LastModified.Should().BeAfter(DateTime.Today);
            result.RecurrenceData.Should().BeNull();
        }
    }
}
