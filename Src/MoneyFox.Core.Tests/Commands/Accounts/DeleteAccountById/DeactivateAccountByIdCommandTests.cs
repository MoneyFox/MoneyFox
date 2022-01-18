using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Core._Pending_.Common;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.DbBackup;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Commands.Accounts.DeleteAccountById;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Commands.Accounts.DeleteAccountById
{
    [ExcludeFromCodeCoverage]
    public class DeactivateAccountByIdCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;
        private readonly Mock<IBackupService> backupServiceMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;

        public DeactivateAccountByIdCommandTests()
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
        public async Task DeactivatedAccountNotDeleted()
        {
            // Arrange
            var account = new Account("test");
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            await new DeactivateAccountByIdCommand.Handler(
                    contextAdapterMock.Object,
                    backupServiceMock.Object,
                    settingsFacadeMock.Object)
                .Handle(new DeactivateAccountByIdCommand(account.Id), default);

            // Assert
            (await context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id)).Should().NotBeNull();
        }

        [Fact]
        public async Task AccoutnDeactivated()
        {
            // Arrange
            var account = new Account("test");
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            await new DeactivateAccountByIdCommand.Handler(
                    contextAdapterMock.Object,
                    backupServiceMock.Object,
                    settingsFacadeMock.Object)
                .Handle(new DeactivateAccountByIdCommand(account.Id), default);

            // Assert
            (await context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id)).IsDeactivated.Should().BeTrue();
        }

        [Fact]
        public async Task UploadeBackupOnDelete()
        {
            // Arrange
            var account = new Account("test");
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            await new DeactivateAccountByIdCommand.Handler(
                    contextAdapterMock.Object,
                    backupServiceMock.Object,
                    settingsFacadeMock.Object)
                .Handle(new DeactivateAccountByIdCommand(account.Id), default);

            // Assert
            backupServiceMock.Verify(x => x.UploadBackupAsync(BackupMode.Automatic), Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>(), Times.Once);
        }
    }
}