using FluentAssertions;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Commands.UpdatePayment;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Commands.UpdatePaymentById
{
    [ExcludeFromCodeCoverage]
    public class UpdatePaymentCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;

        public UpdatePaymentCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);

            backupServiceMock = new Mock<IBackupService>();
            backupServiceMock.Setup(x => x.UploadBackupAsync(BackupMode.Automatic))
                             .Returns(Task.CompletedTask);

            settingsFacadeMock = new Mock<ISettingsFacade>();
            settingsFacadeMock.Setup(x => x.LastDatabaseUpdate);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task UpdatePayment_PaymentFound()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            payment1.UpdatePayment(payment1.Date, 100, payment1.Type, payment1.ChargedAccount);

            // Act
            await new UpdatePaymentCommand.Handler(contextAdapterMock.Object,
                                                   backupServiceMock.Object,
                                                   settingsFacadeMock.Object)
               .Handle(new UpdatePaymentCommand(payment1.Id,
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

        #region Test Recurrence

        [Fact]
        public async Task CategoryForRecurringPaymentUpdated()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80),null);
            payment1.AddRecurringPayment(PaymentRecurrence.Monthly);

            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            var category = new Category("Test");
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            payment1.UpdatePayment(payment1.Date, 100, payment1.Type, payment1.ChargedAccount, category: category);

            // Act
            await new UpdatePaymentCommand.Handler(contextAdapterMock.Object,
                                                   backupServiceMock.Object,
                                                   settingsFacadeMock.Object)
               .Handle(new UpdatePaymentCommand(payment1.Id,
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
            (await context.RecurringPayments.FindAsync(payment1.RecurringPayment.Id)).Category.Id.Should().Be(payment1.Category.Id);
        }

        [Fact]
        public async Task RecurrenceForRecurringPaymentUpdated()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80),null);
            payment1.AddRecurringPayment(PaymentRecurrence.Monthly);

            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            var category = new Category("Test");
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            payment1.UpdatePayment(payment1.Date, 100, payment1.Type, payment1.ChargedAccount, category: category);

            // Act
            await new UpdatePaymentCommand.Handler(contextAdapterMock.Object,
                                                   backupServiceMock.Object,
                                                   settingsFacadeMock.Object)
               .Handle(new UpdatePaymentCommand(payment1.Id,
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
            (await context.RecurringPayments.FindAsync(payment1.RecurringPayment.Id)).Recurrence.Should().Be(PaymentRecurrence.Daily);
        }

        #endregion

        [Fact]
        public async Task UploadBackupOnUpdatePayment()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            payment1.UpdatePayment(payment1.Date, 100, payment1.Type, payment1.ChargedAccount);

            // Act
            await new UpdatePaymentCommand.Handler(contextAdapterMock.Object,
                                                   backupServiceMock.Object,
                                                   settingsFacadeMock.Object)
               .Handle(new UpdatePaymentCommand(payment1.Id,
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
            backupServiceMock.Verify(x => x.UploadBackupAsync(BackupMode.Automatic), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>(), Times.Once);
        }
    }
}
