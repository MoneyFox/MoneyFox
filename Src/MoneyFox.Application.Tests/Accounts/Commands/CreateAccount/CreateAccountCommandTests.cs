using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Commands.CreateAccount;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Commands.CreateAccount
{
    [ExcludeFromCodeCoverage]
    public class CreateAccountCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public CreateAccountCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account("test", 80);

            // Act
            await new CreateAccountCommand.Handler(contextAdapterMock.Object).Handle(new CreateAccountCommand {AccountToSave = account}, default);

            // Assert
            Assert.Single(context.Accounts);
        }
    }
}
