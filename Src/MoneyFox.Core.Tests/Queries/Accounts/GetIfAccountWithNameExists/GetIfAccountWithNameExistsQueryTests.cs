namespace MoneyFox.Core.Tests.Queries.Accounts.GetIfAccountWithNameExists
{
    using Common.Interfaces;
    using Core.Aggregates;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using Moq;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Core.Queries;
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

        protected virtual void Dispose(bool disposing) => InMemoryAppDbContextFactory.Destroy(context);

        [Theory]
        [InlineData("Foo", true)]
        [InlineData("foo", true)]
        [InlineData("Foo212", false)]
        public async Task GetExcludedAccountQuery_CorrectNumberLoaded(string name, bool expectedResult)
        {
            // Arrange
            var accountExcluded = new Account("Foo", 80, isExcluded: true);
            var accountIncluded = new Account("test", 80);
            await context.AddAsync(accountExcluded);
            await context.AddAsync(accountIncluded);
            await context.SaveChangesAsync();

            // Act
            bool result =
                await new GetIfAccountWithNameExistsQuery.Handler(contextAdapterMock.Object)
                    .Handle(new GetIfAccountWithNameExistsQuery(name, 0), default);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}