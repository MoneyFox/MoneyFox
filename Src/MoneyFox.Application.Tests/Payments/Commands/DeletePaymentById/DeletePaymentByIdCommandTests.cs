using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Commands.DeletePaymentById;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Commands.DeletePaymentById
{
    [ExcludeFromCodeCoverage]
    public class DeletePaymentByIdCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IContextAdapter> contextAdapterMock; 
        private readonly Mock<ISettingsFacade> settingsFacadeMock;

        public DeletePaymentByIdCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            backupServiceMock = new Mock<IBackupService>();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);

            settingsFacadeMock = new Mock<ISettingsFacade>();
            settingsFacadeMock.SetupSet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>());
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
        public async Task DeletePayment_PaymentDeleted()
        {
            // Arrange
            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            // Act
            await new DeletePaymentByIdCommand.Handler(contextAdapterMock.Object, backupServiceMock.Object, settingsFacadeMock.Object)
               .Handle(new DeletePaymentByIdCommand(payment1.Id), default);

            // Assert
            Assert.Empty(context.Payments);
        }

        [Fact]
        public async Task DeletePayment_BackupUploaded()
        {
            // Arrange
            backupServiceMock.Setup(x => x.UploadBackupAsync(It.IsAny<BackupMode>()))
                             .Returns(Task.CompletedTask);
            backupServiceMock.Setup(x => x.RestoreBackupAsync(It.IsAny<BackupMode>()))
                             .Returns(Task.CompletedTask);

            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            // Act
            await new DeletePaymentByIdCommand.Handler(contextAdapterMock.Object, backupServiceMock.Object, settingsFacadeMock.Object)
               .Handle(new DeletePaymentByIdCommand(payment1.Id), default);

            // Assert
            backupServiceMock.Verify(x => x.RestoreBackupAsync(It.IsAny<BackupMode>()), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>(), Times.Once);
        }
    }
}
