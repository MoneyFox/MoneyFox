namespace MoneyFox.Core.Tests.Commands.Accounts.UpdateAccount
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using ApplicationCore.Domain.Aggregates.AccountAggregate;
    using Common.Interfaces;
    using Core.Commands.Accounts.UpdateAccount;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class UpdateCategoryCommandTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public UpdateCategoryCommandTests()
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
        public async Task UpdateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account(name: "test", initalBalance: 80);
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            account.UpdateAccount("foo");
            await new UpdateAccountCommand.Handler(contextAdapterMock.Object).Handle(request: new UpdateAccountCommand(account), cancellationToken: default);
            var loadedAccount = await context.Accounts.FindAsync(account.Id);

            // Assert
            loadedAccount.Name.Should().Be("foo");
        }
    }

}
