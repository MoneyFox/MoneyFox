using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccount;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetIncludedAccountQueryTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetIncludedAccountQueryTests()
        {
            context = InMemoryEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetIncludedAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var accountExcluded = new Account("test", 80, isExcluded: true);
            var accountIncluded = new Account("test", 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            List<Account> result = await new GetIncludedAccountQuery.Handler(context).Handle(new GetIncludedAccountQuery(), default);

            // Assert
            Assert.Single(result);
        }
    }
}
