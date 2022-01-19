using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Queries.Accounts.GetIncludedAccount;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Queries.Accounts
{
    [ExcludeFromCodeCoverage]
    public class GetIncludedAccountQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetIncludedAccountQueryTests()
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
        public async Task GetIncludedAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var accountExcluded = new Account("test", 80, isExcluded: true);
            var accountIncluded = new Account("test", 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            List<Account> result =
                await new GetIncludedAccountQuery.Handler(contextAdapterMock.Object).Handle(
                    new GetIncludedAccountQuery(),
                    default);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task DontLoadDeactivatedAccount()
        {
            // Arrange
            var accountExcluded = new Account("test", 80, isExcluded: true);
            var accountIncluded = new Account("test", 80);
            var accountDeactivated = new Account("test", 80);
            accountDeactivated.Deactivate();

            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.AddAsync(accountDeactivated);
            await context.SaveChangesAsync();

            // Act
            List<Account> result =
                await new GetIncludedAccountQuery.Handler(contextAdapterMock.Object).Handle(
                    new GetIncludedAccountQuery(),
                    default);

            // Assert
            Assert.Single(result);
        }
    }
}