namespace MoneyFox.Core.Tests.Commands.Accounts.CreateAccount
{
    using Core._Pending_.Common.Facades;
    using Core._Pending_.Common.Interfaces;
    using Core.Commands.Accounts.CreateAccount;
    using Infrastructure;
    using MediatR;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class CreateAccountCommandTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;
        private readonly Mock<IPublisher> publisherMock;
        private readonly Mock<ISettingsFacade> settingsFacadeMock;

        public CreateAccountCommandTests()
        {
            context = InMemoryAppDbContextFactory.Create();

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

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(context);

        [Fact]
        public async Task GetAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            // Act
            await new CreateAccountCommand.Handler(
                    contextAdapterMock.Object)
                .Handle(
                    new CreateAccountCommand("test", 80),
                    default);

            // Assert
            Assert.Single(context.Accounts);
        }
    }
}