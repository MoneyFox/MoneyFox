namespace MoneyFox.Core.Tests.Queries.Payments.GetPaymentById;

using Core.Queries;
using Domain.Aggregates.AccountAggregate;
using Domain.Exceptions;
using FluentAssertions;

public class GetPaymentByIdQueryTests : InMemoryTestBase
{
    private readonly GetPaymentByIdQuery.Handler handler;

    public GetPaymentByIdQueryTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetPayment_PaymentNotFound()
    {
        // Act / Assert
        await Assert.ThrowsAsync<PaymentNotFoundException>(async () => await handler.Handle(request: new(999), cancellationToken: default));
    }

    [Fact]
    public async Task GetPayment_PaymentFound()
    {
        // Arrange
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new(name: "test", initialBalance: 80));
        await Context.AddAsync(payment1);
        await Context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(payment1.Id), cancellationToken: default);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(payment1.Id);
    }
}
