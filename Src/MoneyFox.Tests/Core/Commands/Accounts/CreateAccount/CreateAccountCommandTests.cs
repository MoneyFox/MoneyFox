namespace MoneyFox.Tests.Core.Commands.Accounts.CreateAccount
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using MoneyFox.Core._Pending_.Common.Facades;
    using MoneyFox.Core.Commands.Accounts.CreateAccount;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class CreateAccountCommandTests
    {
        private readonly AppDbContext context;
        private readonly CreateAccountCommand.Handler handler;

        public CreateAccountCommandTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            new Mock<ISettingsFacade>().SetupSet(x => x.LastDatabaseUpdate = It.IsAny<DateTime>());
            handler = new CreateAccountCommand.Handler(context);
        }

        [Fact]
        public async Task GetAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            // Act
            await handler.Handle(request: new CreateAccountCommand(name: "test", currentBalance: 80), cancellationToken: default);

            // Assert
            Assert.Single(context.Accounts);
        }
    }

}
