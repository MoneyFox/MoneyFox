namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Accounts.GetAccountNameById
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
    public class GetAccountNameByIdQueryTests
    {
        private readonly AppDbContext context;
        private readonly GetAccountNameByIdQuery.Handler handler;

        public GetAccountNameByIdQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            handler = new GetAccountNameByIdQuery.Handler(context);
        }

        [Fact]
        public async Task GetAccountByIdQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account1 = new Account(name: "test2", initialBalance: 80);
            await context.AddAsync(account1);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request: new GetAccountNameByIdQuery(account1.Id), cancellationToken: default);

            // Assert
            result.Should().Be(account1.Name);
        }

        [Fact]
        public async Task EmptyStringWhenNoAccountFound()
        {
            // Act
            var result = await handler.Handle(request: new GetAccountNameByIdQuery(33), cancellationToken: default);

            // Assert
            result.Should().Be(string.Empty);
        }
    }

}
