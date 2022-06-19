namespace MoneyFox.Tests.Core.Commands.Accounts.DeleteAccountById
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.Commands.Accounts.DeleteAccountById;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DeactivateAccountByIdCommandTests
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public DeactivateAccountByIdCommandTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            InMemoryAppDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task DeactivatedAccountNotDeleted()
        {
            // Arrange
            var account = new Account("test");
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            await new DeactivateAccountByIdCommand.Handler(contextAdapterMock.Object).Handle(
                request: new DeactivateAccountByIdCommand(account.Id),
                cancellationToken: default);

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
            await new DeactivateAccountByIdCommand.Handler(contextAdapterMock.Object).Handle(
                request: new DeactivateAccountByIdCommand(account.Id),
                cancellationToken: default);

            // Assert
            (await context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id)).IsDeactivated.Should().BeTrue();
        }
    }

}
