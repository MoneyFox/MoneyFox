using FluentAssertions;
using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.DbBackup;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Commands.Payments.UpdatePayment;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Commands.Payments.UpdatePaymentById
{
    [ExcludeFromCodeCoverage]
    public class UpdatePaymentCommandTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public UpdatePaymentCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

        [Fact]
        public async Task UpdatePayment_PaymentFound()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            payment1.UpdatePayment(payment1.Date, 100, payment1.Type, payment1.ChargedAccount);

            // Act
            await new UpdatePaymentCommand.Handler(
                    contextAdapterMock.Object)
                .Handle(
                    new UpdatePaymentCommand(
                        payment1.Id,
                        payment1.Date,
                        payment1.Amount,
                        payment1.IsCleared,
                        payment1.Type,
                        payment1.Note,
                        payment1.IsRecurring,
                        payment1.Category != null
                            ? payment1.Category.Id
                            : 0,
                        payment1.ChargedAccount != null
                            ? payment1.ChargedAccount.Id
                            : 0,
                        payment1.TargetAccount != null
                            ? payment1.TargetAccount.Id
                            : 0,
                        false,
                        null,
                        null,
                        null),
                    default);

            // Assert
            (await context.Payments.FindAsync(payment1.Id)).Amount.Should().Be(payment1.Amount);
        }

        [Fact]
        public async Task CategoryForRecurringPaymentUpdated()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            payment1.AddRecurringPayment(PaymentRecurrence.Monthly);

            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            var category = new Category("Test");
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            payment1.UpdatePayment(payment1.Date, 100, payment1.Type, payment1.ChargedAccount, category: category);

            // Act
            await new UpdatePaymentCommand.Handler(contextAdapterMock.Object)
                .Handle(
                    new UpdatePaymentCommand(
                        payment1.Id,
                        payment1.Date,
                        payment1.Amount,
                        payment1.IsCleared,
                        payment1.Type,
                        payment1.Note,
                        payment1.IsRecurring,
                        payment1.Category.Id,
                        payment1.ChargedAccount != null
                            ? payment1.ChargedAccount.Id
                            : 0,
                        payment1.TargetAccount != null
                            ? payment1.TargetAccount.Id
                            : 0,
                        true,
                        PaymentRecurrence.Monthly,
                        null,
                        null),
                    default);

            // Assert
            (await context.RecurringPayments.FindAsync(payment1.RecurringPayment.Id)).Category.Id.Should()
                .Be(payment1.Category.Id);
        }

        [Fact]
        public async Task RecurrenceForRecurringPaymentUpdated()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            payment1.AddRecurringPayment(PaymentRecurrence.Monthly);

            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            var category = new Category("Test");
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            payment1.UpdatePayment(payment1.Date, 100, payment1.Type, payment1.ChargedAccount, category: category);

            // Act
            await new UpdatePaymentCommand.Handler(contextAdapterMock.Object)
                .Handle(
                    new UpdatePaymentCommand(
                        payment1.Id,
                        payment1.Date,
                        payment1.Amount,
                        payment1.IsCleared,
                        payment1.Type,
                        payment1.Note,
                        payment1.IsRecurring,
                        payment1.Category.Id,
                        payment1.ChargedAccount != null
                            ? payment1.ChargedAccount.Id
                            : 0,
                        payment1.TargetAccount != null
                            ? payment1.TargetAccount.Id
                            : 0,
                        true,
                        PaymentRecurrence.Daily,
                        null,
                        null),
                    default);

            // Assert
            (await context.RecurringPayments.FindAsync(payment1.RecurringPayment.Id)).Recurrence.Should()
                .Be(PaymentRecurrence.Daily);
        }
    }
}