namespace MoneyFox.Core.Tests.Features._Legacy_.Payments.UpdatePaymentById;

using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core.Common.Settings;
using MoneyFox.Core.Features._Legacy_.Payments.UpdatePayment;
using MoneyFox.Domain.Aggregates.AccountAggregate;
using NSubstitute;

public class UpdatePaymentCommandTests : InMemoryTestBase
{
    private readonly UpdatePayment.Handler handler;

    public UpdatePaymentCommandTests()
    {
        var sender = Substitute.For<ISender>();
        var settings = Substitute.For<ISettingsFacade>();
        handler = new(appDbContext: Context, sender: sender, settings: settings);
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
                IsRecurring: payment1.IsRecurring,
                CategoryId: payment1.Category?.Id ?? 0,
                ChargedAccountId: payment1.ChargedAccount.Id,
                TargetAccountId: payment1.TargetAccount?.Id ?? 0,
                UpdateRecurringPayment: false,
                Recurrence: null,
                EndDate: null,
                IsLastDayOfMonth: false),
            cancellationToken: default);

        // Assert
        (await Context.Payments.SingleAsync(p => p.Id == payment1.Id)).Amount.Should().Be(payment1.Amount);
    }
}
