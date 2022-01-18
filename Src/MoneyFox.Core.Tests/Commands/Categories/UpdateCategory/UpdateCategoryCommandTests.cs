using FluentAssertions;
using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.DbBackup;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Commands.Categories.UpdateCategory;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Commands.Categories.UpdateCategory
{
    [ExcludeFromCodeCoverage]
    public class UpdateCategoryCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IContextAdapter> contextAdapterMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;

        public UpdateCategoryCommandTests()
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
        public async Task UpdateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            var category = new Category("test");
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            category.UpdateData("foo");
            await new UpdateCategoryCommand.Handler(
                    contextAdapterMock.Object,
                    backupServiceMock.Object,
                    settingsFacadeMock.Object)
                .Handle(new UpdateCategoryCommand(category), default);

            Category loadedCategory = await context.Categories.FindAsync(category.Id);

            // Assert
            loadedCategory.Name.Should().Be("foo");
        }

        [Fact]
        public async Task UpdateCategoryCommand_ShouldUpdateRequireNote()
        {
            // Arrange
            var category = new Category("test", requireNote: false);
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            category.UpdateData("foo", requireNote: true);
            await new UpdateCategoryCommand.Handler(
                    contextAdapterMock.Object,
                    backupServiceMock.Object,
                    settingsFacadeMock.Object)
                .Handle(new UpdateCategoryCommand(category), default);

            Category loadedCategory = await context.Categories.FindAsync(category.Id);

            // Assert
            loadedCategory.RequireNote.Should().BeTrue();
        }

        [Fact]
        public async Task SyncDoneOnCreation()
        {
            // Arrange
            backupServiceMock.Setup(x => x.RestoreBackupAsync(It.IsAny<BackupMode>())).Returns(Task.CompletedTask);
            backupServiceMock.Setup(x => x.UploadBackupAsync(It.IsAny<BackupMode>())).Returns(Task.CompletedTask);

            var category = new Category("test");
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            await new UpdateCategoryCommand.Handler(
                    contextAdapterMock.Object,
                    backupServiceMock.Object,
                    settingsFacadeMock.Object)
                .Handle(new UpdateCategoryCommand(category), default);

            // Assert
            backupServiceMock.Verify(x => x.UploadBackupAsync(It.IsAny<BackupMode>()), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>(), Times.Once);
        }
    }
}