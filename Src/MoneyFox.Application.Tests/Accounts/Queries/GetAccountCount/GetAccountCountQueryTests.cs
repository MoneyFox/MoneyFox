using FluentAssertions;
using MoneyFox.Application.Accounts.Queries.GetAccountCount;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries.GetAccountCount
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
            int result = await new GetAccountCountQuery.Handler(contextAdapter).Handle(new GetAccountCountQuery(), default);

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
            int result = await new GetAccountCountQuery.Handler(contextAdapter).Handle(new GetAccountCountQuery(), default);

            // Assert
            result.Should().Be(1);
        }
    }
}
