namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Accounts
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetIncludedAccountBalanceSummaryTests
    {
        private readonly AppDbContext context;
        private readonly GetIncludedAccountBalanceSummaryQuery.Handler handler;

        public GetIncludedAccountBalanceSummaryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new GetIncludedAccountBalanceSummaryQuery.Handler(context);
        }

        [Fact]
        public async Task GetIncludedAccountBalanceSummary_CorrectSum()
        {
            // Arrange
            var accountExcluded = new Account(name: "test", initialBalance: 80, isExcluded: true);
            var accountIncluded1 = new Account(name: "test", initialBalance: 100);
            var accountIncluded2 = new Account(name: "test", initialBalance: 120);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded1);
            await context.AddAsync(accountIncluded2);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request: new GetIncludedAccountBalanceSummaryQuery(), cancellationToken: default);

            // Assert
            result.Should().Be(220);
        }
    }

}
