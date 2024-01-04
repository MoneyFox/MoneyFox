namespace MoneyFox.Core.Tests.Features.UpdatePaymentById;

using Core.Common.Settings;
using Core.Features.RecurringTransactionUpdate;
using Core.Features.UpdatePayment;
using Domain.Aggregates.AccountAggregate;
using Domain.Tests.TestFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdatePaymentCommandTests : InMemoryTestBase
{
    private readonly UpdatePayment.Handler handler;
    private readonly ISender sender;

    public UpdatePaymentCommandTests()
    {
        sender = Substitute.For<ISender>();
        var settings = Substitute.For<ISettingsFacade>();
        settings.DefaultCurrency.Returns("CHF");
        handler = new(appDbContext: Context, sender: sender, settings: settings);
    }

    [Fact]
    public async Task UpdatePayment_PaymentFound()
    {
        // Arrange
        var payment = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new(name: "test", initialBalance: 80));
        await Context.AddAsync(payment);
        await Context.SaveChangesAsync();
        payment.UpdatePayment(date: payment.Date, amount: 100, type: payment.Type, chargedAccount: payment.ChargedAccount);

        // Act
        await handler.Handle(
            command: new(
                Id: payment.Id,
                Date: payment.Date,
                Amount: payment.Amount,
                Type: payment.Type,
                Note: payment.Note!,
                IsRecurring: payment.IsRecurring,
                CategoryId: payment.Category?.Id ?? 0,
                ChargedAccountId: payment.ChargedAccount.Id,
                TargetAccountId: payment.TargetAccount?.Id ?? 0,
                UpdateRecurringPayment: false,
                Recurrence: null,
                EndDate: null,
                IsLastDayOfMonth: false),
            cancellationToken: default);

        // Assert
        (await Context.Payments.SingleAsync(p => p.Id == payment.Id)).Amount.Should().Be(payment.Amount);
    }

    [Fact]
    public async Task UpdateRecurringPaymentWithCorrectAmount()
    {
        // Arrange
        var recurringTransaction = new TestData.RecurringExpense();
        Context.RegisterRecurringTransaction(recurringTransaction);
        var payment = new Payment(
            date: DateTime.Now,
            amount: 20,
            type: PaymentType.Expense,
            chargedAccount: new(name: "test", initialBalance: 80),
            recurringTransactionId: recurringTransaction.RecurringTransactionId);

        await Context.AddAsync(payment);
        await Context.SaveChangesAsync();
        payment.UpdatePayment(date: payment.Date, amount: 100, type: payment.Type, chargedAccount: payment.ChargedAccount);
        var expectedAmount = -payment.Amount;

        // Act
        await handler.Handle(
            command: new(
                Id: payment.Id,
                Date: payment.Date,
                Amount: payment.Amount,
                Type: payment.Type,
                Note: payment.Note!,
                IsRecurring: true,
                CategoryId: payment.Category?.Id ?? 0,
                ChargedAccountId: payment.ChargedAccount.Id,
                TargetAccountId: payment.TargetAccount?.Id ?? 0,
                UpdateRecurringPayment: true,
                Recurrence: PaymentRecurrence.Weekly,
                EndDate: null,
                IsLastDayOfMonth: false),
            cancellationToken: default);

        // Assert
        await sender.Received().Send(Arg.Is<UpdateRecurringTransaction.Command>(e => e.UpdatedAmount.Amount == expectedAmount));
    }
}
