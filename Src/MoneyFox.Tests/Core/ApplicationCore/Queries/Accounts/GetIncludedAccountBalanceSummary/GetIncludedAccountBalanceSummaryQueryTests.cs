namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Accounts.GetIncludedAccountBalanceSummary
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using TestFramework;
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

        protected virtual void Dispose(bool disposing)
        {
            InMemoryAppDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetSummary()
        {
            // Arrange
            var accountExcluded = new Account(name: "test", initialBalance: 80, isExcluded: true);
            var accountIncluded = new Account(name: "test", initialBalance: 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetIncludedAccountBalanceSummaryQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetIncludedAccountBalanceSummaryQuery(),
                cancellationToken: default);

            // Assert
            result.Should().Be(80);
        }

        [Fact]
        public async Task DontIncludeDeactivatedAccounts()
        {
            // Arrange
            var accountExcluded = new Account(name: "test", initialBalance: 80, isExcluded: true);
            var accountIncluded = new Account(name: "test", initialBalance: 80);
            var accountDeactivated = new Account(name: "test", initialBalance: 80);
            accountDeactivated.Deactivate();
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.AddAsync(accountDeactivated);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetIncludedAccountBalanceSummaryQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetIncludedAccountBalanceSummaryQuery(),
                cancellationToken: default);

            // Assert
            result.Should().Be(80);
        }
    }

}
