namespace MoneyFox.Core.Tests.Commands.Payments.UpdatePaymentById;

using Core.Features._Legacy_.Payments.UpdatePayment;
using Domain.Aggregates.AccountAggregate;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class UpdatePaymentCommandTests : InMemoryTestBase
{
    private readonly UpdatePayment.Handler handler;

    public UpdatePaymentCommandTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task UpdatePayment_PaymentFound()
    {
        // Arrange
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new(name: "test", initialBalance: 80));
        await Context.AddAsync(payment1);
        await Context.SaveChangesAsync();
        payment1.UpdatePayment(date: payment1.Date, amount: 100, type: payment1.Type, chargedAccount: payment1.ChargedAccount);

        // Act
        await handler.Handle(
            command: new(
                Id: payment1.Id,
                Date: payment1.Date,
                Amount: payment1.Amount,
                Type: payment1.Type,
                Note: payment1.Note!,
                CategoryId: payment1.Category?.Id ?? 0,
                ChargedAccountId: payment1.ChargedAccount.Id,
                TargetAccountId: payment1.TargetAccount?.Id ?? 0),
            cancellationToken: default);

        // Assert
        (await Context.Payments.SingleAsync(p => p.Id == payment1.Id)).Amount.Should().Be(payment1.Amount);
    }
}
