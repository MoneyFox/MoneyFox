namespace MoneyFox.Core.Tests.Queries.Accounts.GetIncludedAccountBalanceSummary
{
    using Common.Interfaces;
    using Core.Aggregates;
    using Core.Queries.Accounts.GetIncludedAccountBalanceSummary;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetIncludedAccountBalanceSummaryQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetIncludedAccountBalanceSummaryQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(context);

        [Fact]
        public async Task GetSummary()
        {
            // Arrange
            var accountExcluded = new Account("test", 80, isExcluded: true);
            var accountIncluded = new Account("test", 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            decimal result =
                await new GetIncludedAccountBalanceSummaryQuery.Handler(contextAdapterMock.Object)
                    .Handle(new GetIncludedAccountBalanceSummaryQuery(), default);

            // Assert
            result.Should().Be(80);
        }

        [Fact]
        public async Task DontIncludeDeactivatedAccounts()
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
            decimal result =
                await new GetIncludedAccountBalanceSummaryQuery.Handler(contextAdapterMock.Object)
                    .Handle(new GetIncludedAccountBalanceSummaryQuery(), default);

            // Assert
            result.Should().Be(80);
        }
    }
}