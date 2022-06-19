namespace MoneyFox.Tests.Core.Commands.Accounts.UpdateAccount
{

    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.Commands.Accounts.UpdateAccount;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class UpdateCategoryCommandTests
    {
        private readonly AppDbContext context;
        private readonly UpdateAccountCommand.Handler handler;

        public UpdateCategoryCommandTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new UpdateAccountCommand.Handler(context);
        }

        [Fact]
        public async Task UpdateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account(name: "test", initialBalance: 80);
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            account.UpdateAccount("foo");
            await handler.Handle(request: new UpdateAccountCommand(account), cancellationToken: default);
            var loadedAccount = await context.Accounts.FindAsync(account.Id);

            // Assert
            loadedAccount.Name.Should().Be("foo");
        }
    }

}
