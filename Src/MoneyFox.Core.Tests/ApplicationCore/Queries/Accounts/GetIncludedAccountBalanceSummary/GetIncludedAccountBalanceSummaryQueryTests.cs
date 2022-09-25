namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Accounts.GetIncludedAccountBalanceSummary
{

    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetIncludedAccountBalanceSummaryQueryTests
    {
        private readonly AppDbContext context;
        private readonly GetIncludedAccountBalanceSummaryQuery.Handler handler;

        public GetIncludedAccountBalanceSummaryQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new GetIncludedAccountBalanceSummaryQuery.Handler(context);
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
            var result = await handler.Handle(request: new GetIncludedAccountBalanceSummaryQuery(), cancellationToken: default);

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
            var result = await handler.Handle(request: new GetIncludedAccountBalanceSummaryQuery(), cancellationToken: default);

            // Assert
            result.Should().Be(80);
        }
    }

}
