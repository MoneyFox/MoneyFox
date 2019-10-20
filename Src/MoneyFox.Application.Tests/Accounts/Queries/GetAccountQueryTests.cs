using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetAccountQueryTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetAccountQueryTests()
        {
            context = TestEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account("test", 80);
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            List<Account> result = await new GetAccountsQuery.Handler(context).Handle(new GetAccountsQuery(), default);

            // Assert
            Assert.Single(result);
        }
    }
}
