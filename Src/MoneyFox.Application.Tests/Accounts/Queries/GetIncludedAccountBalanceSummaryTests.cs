using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Queries;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetIncludedAccountBalanceSummaryTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetIncludedAccountBalanceSummaryTests()
        {
            context = TestEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }
        
        [Fact]
        public async Task GetIncludedAccountBalanceSummary_CorrectSum()
        {
            // Arrange
            var accountExcluded = new Account("test", 80, isExcluded: true);
            var accountIncluded1 = new Account("test", 100);
            var accountIncluded2 = new Account("test", 120);

            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded1);
            await context.AddAsync(accountIncluded2);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetIncludedAccountBalanceSummary.Handler(context).Handle(new GetIncludedAccountBalanceSummary(), default);

            // Assert
            result.ShouldEqual(220);
        }
    }
}
