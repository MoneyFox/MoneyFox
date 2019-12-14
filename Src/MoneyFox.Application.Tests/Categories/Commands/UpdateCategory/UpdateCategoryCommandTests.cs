using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Categories.Command.UpdateCategory;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Commands.UpdateCategory
{
    [ExcludeFromCodeCoverage]
    public class UpdateCategoryCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public UpdateCategoryCommandTests()
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
        public async Task UpdateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            var category = new Category("test");
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            category.UpdateData("foo");
            await new UpdateCategoryCommand.Handler(contextAdapterMock.Object, backupServiceMock.Object).Handle(new UpdateCategoryCommand {Category = category}, default);

            Category loadedCategory = await context.Categories.FindAsync(category.Id);

            // Assert
            loadedCategory.Name.ShouldEqual("foo");
        }

        [Fact]
        public async Task SyncDoneOnCreation()
        {
            // Arrange
            backupServiceMock.Setup(x => x.RestoreBackupAsync()).Returns(Task.CompletedTask);
            backupServiceMock.Setup(x => x.UploadBackupAsync(It.IsAny<BackupMode>())).Returns(Task.CompletedTask);

            var category = new Category("test");
            await context.AddAsync(category);
            await context.SaveChangesAsync();

            // Act
            await new UpdateCategoryCommand.Handler(contextAdapterMock.Object, backupServiceMock.Object).Handle(new UpdateCategoryCommand { Category = category }, default);

            // Assert
            backupServiceMock.Verify(x => x.UploadBackupAsync(It.IsAny<BackupMode>()), Times.Once);
        }
    }
}
