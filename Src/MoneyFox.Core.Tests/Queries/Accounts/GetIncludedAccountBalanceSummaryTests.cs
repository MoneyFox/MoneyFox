namespace MoneyFox.Core.Tests.Queries.Accounts
{
    using Core._Pending_.Common.Interfaces;
    using Core.Aggregates;
    using Core.Queries.Accounts.GetIncludedAccountBalanceSummary;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
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

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(context);

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
                await new GetIncludedAccountBalanceSummaryQuery.Handler(contextAdapter)
                    .Handle(new GetIncludedAccountBalanceSummaryQuery(), default);

            // Assert
            result.Should().Be(220);
        }
    }
}