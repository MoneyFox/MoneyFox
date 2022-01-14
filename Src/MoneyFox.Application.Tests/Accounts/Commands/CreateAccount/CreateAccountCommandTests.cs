using MoneyFox.Application.Accounts.Commands.CreateAccount;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.DbBackup;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Commands.CreateAccount
{
    [ExcludeFromCodeCoverage]
    public class CreateAccountCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;

        public CreateAccountCommandTests()
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

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

        [Fact]
        public async Task GetAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            // Act
            await new CreateAccountCommand.Handler(
                    contextAdapterMock.Object,
                    backupServiceMock.Object,
                    settingsFacadeMock.Object)
                .Handle(
                    new CreateAccountCommand("test", 80),
                    default);

            // Assert
            Assert.Single(context.Accounts);
        }

        [Fact]
        public async Task BackupUploadedOnCreate()
        {
            // Arrange
            // Act
            await new CreateAccountCommand.Handler(
                    contextAdapterMock.Object,
                    backupServiceMock.Object,
                    settingsFacadeMock.Object)
                .Handle(
                    new CreateAccountCommand("Test", 80),
                    default);

            // Assert
            backupServiceMock.Verify(x => x.UploadBackupAsync(BackupMode.Automatic), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>(), Times.Once);
        }
    }
}