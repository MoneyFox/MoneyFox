namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Accounts.GetIfAccountWithNameExists
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
    public class GetIfAccountWithNameExistsQueryTests
    {
        private readonly AppDbContext context;
        private readonly GetIfAccountWithNameExistsQuery.Handler handler;

        public GetIfAccountWithNameExistsQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new GetIfAccountWithNameExistsQuery.Handler(context);
        }

        [Theory]
        [InlineData("Foo", true)]
        [InlineData("foo", true)]
        [InlineData("Foo212", false)]
        public async Task GetExcludedAccountQuery_CorrectNumberLoaded(string name, bool expectedResult)
        {
            // Arrange
            var accountExcluded = new Account(name: "Foo", initialBalance: 80, isExcluded: true);
            var accountIncluded = new Account(name: "test", initialBalance: 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request: new GetIfAccountWithNameExistsQuery(accountName: name, accountId: 0), cancellationToken: default);

            // Assert
            result.Should().Be(expectedResult);
        }
    }

}
