using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Queries;
using MoneyFox.Application.Accounts.Queries.GetAccountCount;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries.GetAccountCount
{
    [ExcludeFromCodeCoverage]
    public class GetAccountCountQueryTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetAccountCountQueryTests()
        {
            context = TestEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var accountExcluded = new Account("test", 80, isExcluded: true);
            var accountIncluded = new Account("test", 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            int result = await new GetAccountCountQuery.Handler(context).Handle(new GetAccountCountQuery(), default);

            // Assert
            result.ShouldEqual(2);
        }
    }
}
