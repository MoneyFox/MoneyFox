using MoneyFox.Application.Accounts.Commands.UpdateAccount;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Commands.UpdateAccount
{
    [ExcludeFromCodeCoverage]
    public class UpdateCategoryCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;

        public UpdateCategoryCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);

            backupServiceMock = new Mock<IBackupService>();
            backupServiceMock.Setup(x => x.UploadBackupAsync(BackupMode.Automatic))
                             .Returns(Task.CompletedTask);

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
        public async Task UpdateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account("test", 80);
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            account.UpdateAccount("foo");
            await new UpdateAccountCommand.Handler(contextAdapterMock.Object,
                                                   backupServiceMock.Object,
                                                   settingsFacadeMock.Object)
                .Handle(new UpdateAccountCommand(account), default);

            Account loadedAccount = await context.Accounts.FindAsync(account.Id);

            // Assert
            loadedAccount.Name.ShouldEqual("foo");
        }

        [Fact]
        public async Task BackUploadOnUpdate()
        {
            // Arrange
            var account = new Account("test", 80);
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            account.UpdateAccount("foo");
            await new UpdateAccountCommand.Handler(contextAdapterMock.Object,
                                                   backupServiceMock.Object,
                                                   settingsFacadeMock.Object)
                .Handle(new UpdateAccountCommand(account), default);

            Account loadedAccount = await context.Accounts.FindAsync(account.Id);

            // Assert
            backupServiceMock.Verify(x => x.UploadBackupAsync(BackupMode.Automatic), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>(), Times.Once);
        }
    }
}
