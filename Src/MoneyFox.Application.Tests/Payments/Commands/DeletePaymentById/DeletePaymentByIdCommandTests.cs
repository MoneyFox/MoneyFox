using MoneyFox.Application.Common;
using MoneyFox.Application.Common.CloudBackup;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Payments.Commands.DeletePaymentById;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Xunit;
using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Application.Tests.Payments.Commands.DeletePaymentById
{
    [ExcludeFromCodeCoverage]
    public class DeletePaymentByIdCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public DeletePaymentByIdCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            backupServiceMock = new Mock<IBackupService>();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
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
            await new DeletePaymentByIdCommand.Handler(contextAdapterMock.Object, backupServiceMock.Object).Handle(new DeletePaymentByIdCommand(payment1.Id), default);

            // Assert
            Assert.Empty(context.Payments);
        }

        [Fact]
        public async Task DeletePayment_BackupUploaded()
        {
            // Arrange
            backupServiceMock.Setup(x => x.UploadBackupAsync(It.IsAny<BackupMode>()))
                             .Returns(Task.CompletedTask);
            backupServiceMock.Setup(x => x.RestoreBackupAsync())
                             .Returns(Task.CompletedTask);

            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.SaveChangesAsync();

            // Act
            await new DeletePaymentByIdCommand.Handler(contextAdapterMock.Object, backupServiceMock.Object).Handle(new DeletePaymentByIdCommand(payment1.Id), default);

            // Assert
            backupServiceMock.Verify(x => x.RestoreBackupAsync(), Times.Once);
            backupServiceMock.Verify(x => x.UploadBackupAsync(It.IsAny<BackupMode>()), Times.Once);
        }
    }
}
