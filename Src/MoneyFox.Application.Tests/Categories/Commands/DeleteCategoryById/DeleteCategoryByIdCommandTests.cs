using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Categories.Command.DeleteCategoryById;
using MoneyFox.Application.Common;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Categories.Commands.DeleteCategoryById
{
    [ExcludeFromCodeCoverage]
    public class DeleteCategoryByIdCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public DeleteCategoryByIdCommandTests()
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
        public async Task GetExcludedAccountQuery_WithoutFilter_CorrectNumberLoaded()
        {
            // Arrange
            var category1 = new Category("test");
            await context.AddAsync(category1);
            await context.SaveChangesAsync();

            // Act
            await new DeleteCategoryByIdCommand.Handler(contextAdapterMock.Object, backupServiceMock.Object).Handle(new DeleteCategoryByIdCommand(category1.Id), default);

            // Assert
            (await context.Categories.FirstOrDefaultAsync(x => x.Id == category1.Id)).ShouldBeNull();
        }

        [Fact]
        public async Task SyncDoneOnCreation()
        {
            // Arrange
            backupServiceMock.Setup(x => x.RestoreBackupAsync()).Returns(Task.CompletedTask);
            backupServiceMock.Setup(x => x.UploadBackupAsync(It.IsAny<BackupMode>())).Returns(Task.CompletedTask);

            var category1 = new Category("test");
            await context.AddAsync(category1);
            await context.SaveChangesAsync();

            // Act
            await new DeleteCategoryByIdCommand.Handler(contextAdapterMock.Object, backupServiceMock.Object).Handle(new DeleteCategoryByIdCommand(category1.Id), default);

            // Assert
            backupServiceMock.Verify(x => x.RestoreBackupAsync(), Times.Once);
            backupServiceMock.Verify(x => x.UploadBackupAsync(It.IsAny<BackupMode>()), Times.Once);
        }
    }
}
