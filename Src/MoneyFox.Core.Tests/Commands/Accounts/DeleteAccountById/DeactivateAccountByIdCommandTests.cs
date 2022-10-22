namespace MoneyFox.Core.Tests.Commands.Accounts.DeleteAccountById
{

    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.Commands.Accounts.DeleteAccountById;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DeactivateAccountByIdCommandTests
    {
        private readonly AppDbContext context;
        private readonly DeactivateAccountByIdCommand.Handler handler;

        public DeactivateAccountByIdCommandTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new DeactivateAccountByIdCommand.Handler(context);
        }

        [Fact]
        public async Task DeactivatedAccountNotDeleted()
        {
            // Arrange
            var account = new Account("test");
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            await handler.Handle(request: new DeactivateAccountByIdCommand(account.Id), cancellationToken: default);

            // Assert
            (await context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id)).Should().NotBeNull();
        }

        [Fact]
        public async Task AccountDeactivated()
        {
            // Arrange
            var account = new Account("test");
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            await handler.Handle(request: new DeactivateAccountByIdCommand(account.Id), cancellationToken: default);

            // Assert
            (await context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id)).IsDeactivated.Should().BeTrue();
        }
    }

}
