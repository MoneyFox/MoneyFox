namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Payments.GetPaymentById;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Domain.Exceptions;
using Core.ApplicationCore.Queries;
using FluentAssertions;
using Infrastructure.Persistence;
using TestFramework;

[ExcludeFromCodeCoverage]
public class GetPaymentByIdQueryTests
{
    private readonly AppDbContext context;
    private readonly GetPaymentByIdQuery.Handler handler;

    public GetPaymentByIdQueryTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
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
        await context.AddAsync(payment1);
        await context.SaveChangesAsync();

        // Act
        var result = await handler.Handle(request: new(payment1.Id), cancellationToken: default);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(payment1.Id);
    }
}
