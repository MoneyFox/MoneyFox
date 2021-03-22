using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetAccountQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetAccountQueryTests()
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

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

        [Fact]
        public async Task GetAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account("test", 80);
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            List<Account> result = await new GetAccountsQuery.Handler(contextAdapterMock.Object).Handle(new GetAccountsQuery(), default);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task DontLoadDeactivatedAccounts()
        {
            // Arrange
            var account1 = new Account("test", 80);
            var account2 = new Account("test", 80);

            account2.Deactivate();

            await context.AddAsync(account1);
            await context.AddAsync(account2);
            await context.SaveChangesAsync();

            // Act
            List<Account> result = await new GetAccountsQuery.Handler(contextAdapterMock.Object).Handle(new GetAccountsQuery(), default);

            // Assert
            Assert.Single(result);
        }
    }
}
