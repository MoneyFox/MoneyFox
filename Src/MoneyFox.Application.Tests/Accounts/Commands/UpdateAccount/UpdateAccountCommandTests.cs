using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Commands.UpdateAccount;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Commands.UpdateAccount
{
    [ExcludeFromCodeCoverage]
    public class UpdateCategoryCommandTests : IDisposable
    {
        private readonly EfCoreContext context;

        public UpdateCategoryCommandTests()
        {
            context = TestEfCoreContextFactory.Create();
        }
        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task UpdateCategoryCommand_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account("test", 80);
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            account.UpdateAccount("foo");
            await new UpdateAccountCommand.Handler(context).Handle(new UpdateAccountCommand { Account = account }, default);

            var loadedAccount = await context.Accounts.FindAsync(account.Id);

            // Assert
            loadedAccount.Name.ShouldEqual("foo");
        }
    }
}
