namespace MoneyFox.Core.Tests.Commands.Payments.DeletePaymentById;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Commands.Payments.DeletePaymentById;
using Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class DeletePaymentByIdCommandTests : InMemoryTestBase
{
    private readonly DeletePaymentByIdCommand.Handler handler;

    public DeletePaymentByIdCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task DontThrowExceptionWhenIdNotFound()
    {
        // Arrange
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new(name: "test", initialBalance: 80));
        await Context.AddAsync(payment1);
        await Context.SaveChangesAsync();

        // Act
        await handler.Handle(request: new(12), cancellationToken: default);
    }

    [Fact]
    public async Task PaymentIsRemovedWhenAnEntryWithPassedIdExists()
    {
        // Arrange
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new(name: "test", initialBalance: 80));
        await Context.AddAsync(payment1);
        await Context.SaveChangesAsync();

        // Act
        await handler.Handle(request: new(payment1.Id), cancellationToken: default);

        // Assert
        Assert.Empty(Context.Payments);
    }
}
