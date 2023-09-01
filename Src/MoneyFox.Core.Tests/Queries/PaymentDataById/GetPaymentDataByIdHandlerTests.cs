namespace MoneyFox.Core.Tests.Queries.PaymentDataById;

using Core.Queries.PaymentDataById;
using Domain.Tests.TestFramework;
using FluentAssertions.Execution;

public class GetPaymentDataByIdHandlerTests : InMemoryTestBase
{
    private readonly GetPaymentDataById.Handler handler;

    protected GetPaymentDataByIdHandlerTests()
    {
        handler = new(Context);
    }

    private static void AssertPaymentData(PaymentData actual, TestData.IPayment expectedPayment, TestData.IRecurringTransaction? recurringTransaction)
    {
        using (new AssertionScope())
        {
            actual.PaymentId.Should().Be(expectedPayment.Id);
            actual.Amount.Should().Be(expectedPayment.Amount);
            actual.ChargedAccount.Name.Should().Be(expectedPayment.ChargedAccount.Name);
            actual.ChargedAccount.CurrentBalance.Should().Be(expectedPayment.ChargedAccount.CurrentBalance);
            actual.Date.Should().Be(expectedPayment.Date);
            actual.IsCleared.Should().Be(expectedPayment.IsCleared);
            actual.Type.Should().Be(expectedPayment.Type);
            actual.Note.Should().Be(expectedPayment.Note);
            actual.Created.Should().BeAfter(DateTime.Today);
            actual.LastModified.Should().BeAfter(DateTime.Today);
            if (actual.TargetAccount is not null)
            {
                actual.TargetAccount.Name.Should().Be(expectedPayment.TargetAccount!.Name);
                actual.TargetAccount.CurrentBalance.Should().Be(expectedPayment.TargetAccount.CurrentBalance);
            }
            else
            {
                actual.TargetAccount.Should().BeNull();
            }

            if (actual.Category is not null)
            {
                actual.Category.Name.Should().Be(expectedPayment.Category!.Name);
            }
            else
            {
                actual.Category.Should().BeNull();
            }

            if (recurringTransaction is not null)
            {
                actual.RecurrenceData!.Recurrence.Should().Be(recurringTransaction.Recurrence);
                actual.RecurrenceData!.StartDate.Should().Be(recurringTransaction.StartDate);
                actual.RecurrenceData!.EndDate.Should().Be(recurringTransaction.EndDate);
            }
            else
            {
                actual.RecurrenceData.Should().BeNull();
            }
        }
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

    public sealed class OnTimeTransaction : GetPaymentDataByIdHandlerTests
    {
        [Fact]
        public async Task ReturnsCorrectData()
        {
            // Arrange
            var category = new TestData.CategoryBeverages();
            Context.RegisterCategory(category);
            var payment = new TestData.UnclearedExpense();
            Context.RegisterPayment(payment);

            // Act
            var result = await handler.Handle(query: new(payment.Id), cancellationToken: default);

            // Assert
            AssertPaymentData(actual: result, expectedPayment: payment, recurringTransaction: null);
        }
    }

    public sealed class RecurringTimeTransaction : GetPaymentDataByIdHandlerTests
    {
        [Fact]
        public async Task ReturnsCorrectData()
        {
            // Arrange
            var category = new TestData.CategoryBeverages();
            Context.RegisterCategory(category);
            var recurringTransaction = new TestData.RecurringExpense();
            Context.RegisterRecurringTransaction(recurringTransaction);
            var payment = new TestData.UnclearedExpense { RecurringTransactionId = recurringTransaction.RecurringTransactionId };
            Context.RegisterPayment(payment);

            // Act
            var result = await handler.Handle(query: new(payment.Id), cancellationToken: default);

            // Assert
            AssertPaymentData(actual: result, expectedPayment: payment, recurringTransaction: recurringTransaction);
        }
    }
}
