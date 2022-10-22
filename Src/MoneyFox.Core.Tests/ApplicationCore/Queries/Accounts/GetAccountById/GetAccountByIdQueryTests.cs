namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts.GetAccountById
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Infrastructure.Persistence;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetAccountByIdQueryTests : IDisposable
    {
        private readonly AppDbContext context;

        public GetAccountByIdQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
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
        public async Task GetAccountByIdQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account1 = new Account(name: "test2", initialBalance: 80);
            var account2 = new Account(name: "test3", initialBalance: 80);
            await context.AddAsync(account1);
            await context.AddAsync(account2);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetAccountByIdQuery.Handler(context).Handle(request: new GetAccountByIdQuery(account1.Id), cancellationToken: default);

            // Assert
            result.Name.Should().Be(account1.Name);
        }
    }

}
