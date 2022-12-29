namespace MoneyFox.Core.Tests.Commands.Payments.DeletePaymentById;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Domain.Exceptions;
using Core.Commands.Payments.DeletePaymentById;
using Infrastructure.Persistence;
using TestFramework;

[ExcludeFromCodeCoverage]
public class DeletePaymentByIdCommandTests
{
    private readonly AppDbContext context;
    private readonly DeletePaymentByIdCommand.Handler handler;

    public DeletePaymentByIdCommandTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task ThrowExceptionWhenPaymentNotFound()
    {
        // Act / Assert
        // Arrange
        await Assert.ThrowsAsync<PaymentNotFoundException>(async () => await handler.Handle(request: new(12), cancellationToken: default));
    }

    [Fact]
    public async Task DeletePayment_PaymentDeleted()
    {
        // Arrange
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new(name: "test", initialBalance: 80));
        await context.AddAsync(payment1);
        await context.SaveChangesAsync();

        // Act
        await handler.Handle(request: new(payment1.Id), cancellationToken: default);

        // Assert
        Assert.Empty(context.Payments);
    }
}
