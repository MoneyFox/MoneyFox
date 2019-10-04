using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Commands.DeleteAccountById;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Commands.DeleteAccountById
{
    [ExcludeFromCodeCoverage]
    public class DeleteAccountByIdCommandTests : IDisposable
    {
        private readonly EfCoreContext context;

        public DeleteAccountByIdCommandTests()
        {
            context = TestEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_WithoutFilter_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account("test");
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            await new DeleteAccountByIdCommand.Handler(context).Handle(new DeleteAccountByIdCommand(account.Id), default);

            // Assert
            Assert.Single(context.Categories);
        }
    }
}
