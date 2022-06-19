namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Accounts
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetAccountQueryTests
    {
        private readonly AppDbContext context;
        private readonly GetAccountsQuery.Handler handler;

        public GetAccountQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new GetAccountsQuery.Handler(context);
        }

        [Fact]
        public async Task GetAccountQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account(name: "test", initialBalance: 80);
            await context.AddAsync(account);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request: new GetAccountsQuery(), cancellationToken: default);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task DontLoadDeactivatedAccounts()
        {
            // Arrange
            var account1 = new Account(name: "test", initialBalance: 80);
            var account2 = new Account(name: "test", initialBalance: 80);
            account2.Deactivate();
            await context.AddAsync(account1);
            await context.AddAsync(account2);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request: new GetAccountsQuery(), cancellationToken: default);

            // Assert
            Assert.Single(result);
        }
    }

}
