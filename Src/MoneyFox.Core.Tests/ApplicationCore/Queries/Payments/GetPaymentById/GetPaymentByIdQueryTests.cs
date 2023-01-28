namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Payments.GetPaymentById;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Domain.Exceptions;
using Core.ApplicationCore.Queries;
using FluentAssertions;

public class GetPaymentByIdQueryTests : InMemoryTestBase
{
    private readonly GetPaymentByIdQuery.Handler handler;

    public GetPaymentByIdQueryTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task GetCategory_CategoryNotFound()
    {
        // Act / Assert
        // Arrange
        await Assert.ThrowsAsync<PaymentNotFoundException>(async () => await handler.Handle(request: new(999), cancellationToken: default));
    }

    [Fact]
    public async Task GetCategory_CategoryFound()
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
