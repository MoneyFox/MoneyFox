namespace MoneyFox.Core.Tests.Queries.Accounts.GetAccountCount
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Core.Aggregates;
    using Core.Aggregates.AccountAggregate;
    using Core.Queries;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetAccountCountQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly IContextAdapter contextAdapter;

        public GetAccountCountQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            contextAdapter = Substitute.For<IContextAdapter>();
            contextAdapter.Context.Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            InMemoryAppDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var accountExcluded = new Account(name: "test", initalBalance: 80, isExcluded: true);
            var accountIncluded = new Account(name: "test", initalBalance: 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetAccountCountQuery.Handler(contextAdapter).Handle(request: new GetAccountCountQuery(), cancellationToken: default);

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        public async Task HandleDeactivatedAccountsCorrectly()
        {
            // Arrange
            var account = new Account(name: "test", initalBalance: 80);
            var accountDeactivated = new Account(name: "test", initalBalance: 80);
            accountDeactivated.Deactivate();
            await context.AddAsync(accountDeactivated);
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetAccountCountQuery.Handler(contextAdapter).Handle(request: new GetAccountCountQuery(), cancellationToken: default);

            // Assert
            result.Should().Be(1);
        }
    }

}
