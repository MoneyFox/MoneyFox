using FluentAssertions;
using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.DbBackup;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Commands.Categories.CreateCategory;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Commands.Categories.CreateCategory
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

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

        [Fact]
        public async Task CreateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            // Act
            await new CreateCategoryCommand.Handler(
                    contextAdapterMock.Object,
                    backupServiceMock.Object,
                    settingsFacadeMock.Object)
                .Handle(new CreateCategoryCommand("Test"), default);

            // Assert
            Assert.Single(context.Categories);
        }

        [Fact]
        public async Task CreateCategoryCommand_ShouldSaveRequireNoteCorretly()
        {
            // Arrange
            // Act
            await new CreateCategoryCommand.Handler(
                    contextAdapterMock.Object,
                    backupServiceMock.Object,
                    settingsFacadeMock.Object)
                .Handle(new CreateCategoryCommand("Test", requireNote: true), default);

            Category loadedCategory = context.Categories.First();

            // Assert
            loadedCategory.RequireNote.Should().BeTrue();
        }

        [Fact]
        public async Task SyncDoneOnCreation()
        {
            // Arrange
            backupServiceMock.Setup(x => x.RestoreBackupAsync(It.IsAny<BackupMode>())).Returns(Task.CompletedTask);
            backupServiceMock.Setup(x => x.UploadBackupAsync(It.IsAny<BackupMode>())).Returns(Task.CompletedTask);

            // Act
            await new CreateCategoryCommand.Handler(
                    contextAdapterMock.Object,
                    backupServiceMock.Object,
                    settingsFacadeMock.Object)
                .Handle(new CreateCategoryCommand("test"), default);

            // Assert
            backupServiceMock.Verify(x => x.RestoreBackupAsync(It.IsAny<BackupMode>()), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>(), Times.Once);
        }
    }
}