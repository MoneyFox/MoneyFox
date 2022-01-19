using FluentAssertions;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Queries.Accounts.GetAccountCount;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Queries.Accounts.GetAccountCount
{
    [ExcludeFromCodeCoverage]
    public class GetAccountCountQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly IContextAdapter contextAdapter;

        public GetAccountCountQueryTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapter = Substitute.For<IContextAdapter>();
            contextAdapter.Context.Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

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
            int result =
                await new GetAccountCountQuery.Handler(contextAdapter).Handle(new GetAccountCountQuery(), default);

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        public async Task HandleDeactivatedAccountsCorrectly()
        {
            // Arrange
            var account = new Account("test", 80);
            var accountDeactivated = new Account("test", 80);
            accountDeactivated.Deactivate();

            await context.AddAsync(accountDeactivated);
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            int result =
                await new GetAccountCountQuery.Handler(contextAdapter).Handle(new GetAccountCountQuery(), default);

            // Assert
            result.Should().Be(1);
        }
    }
}