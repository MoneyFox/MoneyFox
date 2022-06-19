namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Accounts.GetAccountCount
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
    public class GetAccountCountQueryTests
    {
        private readonly AppDbContext context;
        private readonly IContextAdapter contextAdapter;

        public GetAccountCountQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            contextAdapter = Substitute.For<IContextAdapter>();
            contextAdapter.Context.Returns(context);
        }

        [Fact]
        public async Task GetExcludedAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var accountExcluded = new Account(name: "test", initialBalance: 80, isExcluded: true);
            var accountIncluded = new Account(name: "test", initialBalance: 80);
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
            var account = new Account(name: "test", initialBalance: 80);
            var accountDeactivated = new Account(name: "test", initialBalance: 80);
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
