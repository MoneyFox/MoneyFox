using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Queries.GetIncludedAccountBalanceSummary;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetIncludedAccountBalanceSummaryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetIncludedAccountBalanceSummaryTests()
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
            decimal result =
                await new GetIncludedAccountBalanceSummaryQuery.Handler(contextAdapterMock.Object)
                   .Handle(new GetIncludedAccountBalanceSummaryQuery(), default);

            // Assert
            result.ShouldEqual(220);
        }
    }
}
