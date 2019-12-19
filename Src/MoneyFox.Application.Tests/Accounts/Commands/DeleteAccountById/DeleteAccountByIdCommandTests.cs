using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyFox.Application.Accounts.Commands.DeleteAccountById;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Commands.DeleteAccountById
{
    [ExcludeFromCodeCoverage]
    public class DeleteAccountByIdCommandTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public DeleteAccountByIdCommandTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

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
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_WithoutFilter_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account("test");
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            await new DeleteAccountByIdCommand.Handler(contextAdapterMock.Object).Handle(new DeleteAccountByIdCommand(account.Id), default);

            // Assert
            (await context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id)).ShouldBeNull();
        }
    }
}
