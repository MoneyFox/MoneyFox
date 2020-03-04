using MoneyFox.Application.Categories.Command.CreateCategory;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Commands.CreateCategory
{
    [ExcludeFromCodeCoverage]
    public class CreateCategoryCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IContextAdapter> contextAdapterMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;

        public CreateCategoryCommandTests()
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
        public async Task CreateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            var category = new Category("test");

            // Act
            await new CreateCategoryCommand.Handler(contextAdapterMock.Object, backupServiceMock.Object, settingsFacadeMock.Object)
               .Handle(new CreateCategoryCommand(category), default);

            // Assert
            Assert.Single(context.Categories);
        }

        [Fact]
        public async Task SyncDoneOnCreation()
        {
            // Arrange
            backupServiceMock.Setup(x => x.RestoreBackupAsync(It.IsAny<BackupMode>())).Returns(Task.CompletedTask);
            backupServiceMock.Setup(x => x.UploadBackupAsync(It.IsAny<BackupMode>())).Returns(Task.CompletedTask);

            var category = new Category("test");

            // Act
            await new CreateCategoryCommand.Handler(contextAdapterMock.Object, backupServiceMock.Object, settingsFacadeMock.Object)
               .Handle(new CreateCategoryCommand(category), default);

            // Assert
            backupServiceMock.Verify(x => x.RestoreBackupAsync(It.IsAny<BackupMode>()), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>(), Times.Once);
        }
    }
}
