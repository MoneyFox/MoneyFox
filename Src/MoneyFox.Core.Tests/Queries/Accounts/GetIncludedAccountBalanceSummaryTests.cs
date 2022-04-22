namespace MoneyFox.Core.Tests.Queries.Accounts
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using ApplicationCore.Domain.Aggregates.AccountAggregate;
    using ApplicationCore.Queries;
    using Common.Interfaces;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetIncludedAccountBalanceSummaryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly IContextAdapter contextAdapter;

        public GetIncludedAccountBalanceSummaryTests()
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
        public async Task GetIncludedAccountBalanceSummary_CorrectSum()
        {
            // Arrange
            var accountExcluded = new Account(name: "test", initalBalance: 80, isExcluded: true);
            var accountIncluded1 = new Account(name: "test", initalBalance: 100);
            var accountIncluded2 = new Account(name: "test", initalBalance: 120);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded1);
            await context.AddAsync(accountIncluded2);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetIncludedAccountBalanceSummaryQuery.Handler(contextAdapter).Handle(
                request: new GetIncludedAccountBalanceSummaryQuery(),
                cancellationToken: default);

            // Assert
            result.Should().Be(220);
        }
    }

}
