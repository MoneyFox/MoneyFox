namespace MoneyFox.Core.Tests.Queries.Accounts.GetIfAccountWithNameExists
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
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetIfAccountWithNameExistsQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetIfAccountWithNameExistsQueryTests()
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

        [Theory]
        [InlineData("Foo", true)]
        [InlineData("foo", true)]
        [InlineData("Foo212", false)]
        public async Task GetExcludedAccountQuery_CorrectNumberLoaded(string name, bool expectedResult)
        {
            // Arrange
            var accountExcluded = new Account(name: "Foo", initalBalance: 80, isExcluded: true);
            var accountIncluded = new Account(name: "test", initalBalance: 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetIfAccountWithNameExistsQuery.Handler(contextAdapterMock.Object).Handle(
                request: new GetIfAccountWithNameExistsQuery(accountName: name, accountId: 0),
                cancellationToken: default);

            // Assert
            result.Should().Be(expectedResult);
        }
    }

}
