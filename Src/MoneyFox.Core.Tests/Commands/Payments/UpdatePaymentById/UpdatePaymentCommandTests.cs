namespace MoneyFox.Core.Tests.Commands.Payments.UpdatePaymentById;

using System.Diagnostics.CodeAnalysis;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Domain.Aggregates.CategoryAggregate;
using Core.Commands.Payments.CreateRecurringPayments;
using Core.Commands.Payments.UpdatePayment;
using FluentAssertions;
using Infrastructure.Persistence;

[ExcludeFromCodeCoverage]
public class UpdatePaymentCommandTests
{
    private readonly AppDbContext context;
    private readonly UpdatePayment.Handler handler;

    public UpdatePaymentCommandTests()
    {
        context = InMemoryAppDbContextFactory.Create();
        handler = new(context);
    }

    [Fact]
    public async Task UpdatePayment_PaymentFound()
    {
        // Arrange
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new(name: "test", initialBalance: 80));
        await context.AddAsync(payment1);
        await context.SaveChangesAsync();
        payment1.UpdatePayment(date: payment1.Date, amount: 100, type: payment1.Type, chargedAccount: payment1.ChargedAccount);

        // Act
        await handler.Handle(
            command: new(
                Id: payment1.Id,
                Date: payment1.Date,
                Amount: payment1.Amount,
                IsCleared: payment1.IsCleared,
                Type: payment1.Type,
                Note: payment1.Note,
                IsRecurring: payment1.IsRecurring,
                CategoryId: payment1.Category != null ? payment1.Category.Id : 0,
                ChargedAccountId: payment1.ChargedAccount != null ? payment1.ChargedAccount.Id : 0,
                TargetAccountId: payment1.TargetAccount != null ? payment1.TargetAccount.Id : 0,
                UpdateRecurringPayment: false,
                Recurrence: null,
                IsEndless: null,
                EndDate: null,
                IsLastDayOfMonth: false),
            cancellationToken: default);

        // Assert
        (await context.Payments.FindAsync(payment1.Id)).Amount.Should().Be(payment1.Amount);
    }

    [Fact]
    public async Task CategoryForRecurringPaymentUpdated()
    {
        // Arrange
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new(name: "test", initialBalance: 80));
        payment1.AddRecurringPayment(recurrence: PaymentRecurrence.Monthly, isLastDayOfMonth: false);
        await context.AddAsync(payment1);
        await context.SaveChangesAsync();
        var category = new Category("Test");
        await context.AddAsync(category);
        await context.SaveChangesAsync();
        payment1.UpdatePayment(
            date: payment1.Date,
            amount: 100,
            type: payment1.Type,
            chargedAccount: payment1.ChargedAccount,
            category: category);

        // Act
        await handler.Handle(
            command: new(
                Id: payment1.Id,
                Date: payment1.Date,
                Amount: payment1.Amount,
                IsCleared: payment1.IsCleared,
                Type: payment1.Type,
                Note: payment1.Note,
                IsRecurring: payment1.IsRecurring,
                CategoryId: payment1.Category.Id,
                ChargedAccountId: payment1.ChargedAccount.Id,
                TargetAccountId: payment1.TargetAccount?.Id ?? 0,
                UpdateRecurringPayment: true,
                Recurrence: PaymentRecurrence.Monthly,
                IsEndless: null,
                EndDate: null,
                IsLastDayOfMonth: false),
            cancellationToken: default);

        // Assert
        (await context.RecurringPayments.FindAsync(payment1.RecurringPayment.Id)).Category.Id.Should().Be(payment1.Category.Id);
    }

    [Fact]
    public async Task RecurrenceForRecurringPaymentUpdated()
    {
        // Arrange
        var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new(name: "test", initialBalance: 80));
        payment1.AddRecurringPayment(recurrence: PaymentRecurrence.Monthly, isLastDayOfMonth: false);
        await context.AddAsync(payment1);
        await context.SaveChangesAsync();
        var category = new Category("Test");
        await context.AddAsync(category);
        await context.SaveChangesAsync();
        payment1.UpdatePayment(
            date: payment1.Date,
            amount: 100,
            type: payment1.Type,
            chargedAccount: payment1.ChargedAccount,
            category: category);

        // Act
        await handler.Handle(
            command: new(
                Id: payment1.Id,
                Date: payment1.Date,
                Amount: payment1.Amount,
                IsCleared: payment1.IsCleared,
                Type: payment1.Type,
                Note: payment1.Note,
                IsRecurring: payment1.IsRecurring,
                CategoryId: payment1.Category.Id,
                ChargedAccountId: payment1.ChargedAccount != null ? payment1.ChargedAccount.Id : 0,
                TargetAccountId: payment1.TargetAccount != null ? payment1.TargetAccount.Id : 0,
                UpdateRecurringPayment: true,
                Recurrence: PaymentRecurrence.Daily,
                IsEndless: null,
                EndDate: null,
                IsLastDayOfMonth: false),
            cancellationToken: default);

        // Assert
        (await context.RecurringPayments.FindAsync(payment1.RecurringPayment.Id)).Recurrence.Should().Be(PaymentRecurrence.Daily);
    }

    [Fact]
    public async Task ChangeRecurringToNonRecurringPayment()
    {
        // Arrange
        var payment1 = new Payment(
            date: DateTime.Now.AddDays(-1),
            amount: 20,
            type: PaymentType.Expense,
            chargedAccount: new(name: "test", initialBalance: 80));

        payment1.AddRecurringPayment(recurrence: PaymentRecurrence.Daily);
        await context.AddAsync(payment1);
        await context.SaveChangesAsync();

        // Trigger creation of recurring payment transactions
        await new CreateRecurringPaymentsCommand.Handler(context).Handle(request: new(), cancellationToken: default);

        // Disable recurrence on the payment
        await handler.Handle(
            command: new(
                Id: payment1.Id,
                Date: payment1.Date,
                Amount: payment1.Amount,
                IsCleared: payment1.IsCleared,
                Type: payment1.Type,
                Note: payment1.Note,
                IsRecurring: false,
                CategoryId: 0,
                ChargedAccountId: payment1.ChargedAccount != null ? payment1.ChargedAccount.Id : 0,
                TargetAccountId: payment1.TargetAccount != null ? payment1.TargetAccount.Id : 0,
                UpdateRecurringPayment: true,
                Recurrence: PaymentRecurrence.Daily,
                IsEndless: null,
                EndDate: null,
                IsLastDayOfMonth: false),
            cancellationToken: default);

        // Assert
        var loadedPayments = context.Payments.ToList();
        loadedPayments.Should().HaveCount(2);
        loadedPayments.ForEach(x => x.IsRecurring.Should().BeFalse());
    }
}
