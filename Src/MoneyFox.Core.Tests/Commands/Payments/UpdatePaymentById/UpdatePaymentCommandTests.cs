namespace MoneyFox.Core.Tests.Commands.Payments.UpdatePaymentById
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Core.Aggregates;
    using Core.Aggregates.AccountAggregate;
    using Core.Commands.Payments.UpdatePayment;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class UpdatePaymentCommandTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public UpdatePaymentCommandTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            InMemoryAppDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task UpdatePayment_PaymentFound()
        {
            // Arrange
            var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new Account(name: "test", initalBalance: 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();
            payment1.UpdatePayment(date: payment1.Date, amount: 100, type: payment1.Type, chargedAccount: payment1.ChargedAccount);

            // Act
            await new UpdatePaymentCommand.Handler(contextAdapterMock.Object).Handle(
                request: new UpdatePaymentCommand(
                    id: payment1.Id,
                    date: payment1.Date,
                    amount: payment1.Amount,
                    isCleared: payment1.IsCleared,
                    type: payment1.Type,
                    note: payment1.Note,
                    isRecurring: payment1.IsRecurring,
                    categoryId: payment1.Category != null ? payment1.Category.Id : 0,
                    chargedAccountId: payment1.ChargedAccount != null ? payment1.ChargedAccount.Id : 0,
                    targetAccountId: payment1.TargetAccount != null ? payment1.TargetAccount.Id : 0,
                    updateRecurringPayment: false,
                    recurrence: null,
                    isEndless: null,
                    endDate: null,
                    isLastDayOfMonth: false),
                cancellationToken: default);

            // Assert
            (await context.Payments.FindAsync(payment1.Id)).Amount.Should().Be(payment1.Amount);
        }

        [Fact]
        public async Task CategoryForRecurringPaymentUpdated()
        {
            // Arrange
            var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new Account(name: "test", initalBalance: 80));
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
            await new UpdatePaymentCommand.Handler(contextAdapterMock.Object).Handle(
                request: new UpdatePaymentCommand(
                    id: payment1.Id,
                    date: payment1.Date,
                    amount: payment1.Amount,
                    isCleared: payment1.IsCleared,
                    type: payment1.Type,
                    note: payment1.Note,
                    isRecurring: payment1.IsRecurring,
                    categoryId: payment1.Category.Id,
                    chargedAccountId: payment1.ChargedAccount != null ? payment1.ChargedAccount.Id : 0,
                    targetAccountId: payment1.TargetAccount != null ? payment1.TargetAccount.Id : 0,
                    updateRecurringPayment: true,
                    recurrence: PaymentRecurrence.Monthly,
                    isEndless: null,
                    endDate: null,
                    isLastDayOfMonth: false),
                cancellationToken: default);

            // Assert
            (await context.RecurringPayments.FindAsync(payment1.RecurringPayment.Id)).Category.Id.Should().Be(payment1.Category.Id);
        }

        [Fact]
        public async Task RecurrenceForRecurringPaymentUpdated()
        {
            // Arrange
            var payment1 = new Payment(date: DateTime.Now, amount: 20, type: PaymentType.Expense, chargedAccount: new Account(name: "test", initalBalance: 80));
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
            await new UpdatePaymentCommand.Handler(contextAdapterMock.Object).Handle(
                request: new UpdatePaymentCommand(
                    id: payment1.Id,
                    date: payment1.Date,
                    amount: payment1.Amount,
                    isCleared: payment1.IsCleared,
                    type: payment1.Type,
                    note: payment1.Note,
                    isRecurring: payment1.IsRecurring,
                    categoryId: payment1.Category.Id,
                    chargedAccountId: payment1.ChargedAccount != null ? payment1.ChargedAccount.Id : 0,
                    targetAccountId: payment1.TargetAccount != null ? payment1.TargetAccount.Id : 0,
                    updateRecurringPayment: true,
                    recurrence: PaymentRecurrence.Daily,
                    isEndless: null,
                    endDate: null,
                    isLastDayOfMonth: false),
                cancellationToken: default);

            // Assert
            (await context.RecurringPayments.FindAsync(payment1.RecurringPayment.Id)).Recurrence.Should().Be(PaymentRecurrence.Daily);
        }
    }

}
