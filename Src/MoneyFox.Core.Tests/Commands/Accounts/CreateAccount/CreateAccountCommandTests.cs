using MediatR;
using MoneyFox.Core._Pending_.Common.Facades;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Commands.Accounts.CreateAccount;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Commands.Accounts.CreateAccount
{
    [ExcludeFromCodeCoverage]
    public class CreateAccountCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;
        private readonly Mock<IPublisher> publisherMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;

        public CreateAccountCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);

            publisherMock = new Mock<IPublisher>();

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
                    publisherMock.Object,
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
                    publisherMock.Object,
                    settingsFacadeMock.Object)
                .Handle(
                    new CreateAccountCommand("Test", 80),
                    default);

            // Assert
            // publisherMock.Verify(x => x.Publish(It.IsAny<AccountCreatedEvent>(), It.IsAny<CancellationToken>()),
            //     Times.Once);
            settingsFacadeMock.VerifySet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>(), Times.Once);
        }
    }
}