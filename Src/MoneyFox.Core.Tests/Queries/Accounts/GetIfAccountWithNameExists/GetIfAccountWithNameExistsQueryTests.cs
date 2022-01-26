using FluentAssertions;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Queries.Accounts.GetIfAccountWithNameExists;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Queries.Accounts.GetIfAccountWithNameExists
{
    [ExcludeFromCodeCoverage]
    public class GetIfAccountWithNameExistsQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetIfAccountWithNameExistsQueryTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = new Mock<IContextAdapter>();
            contextAdapterMock.SetupGet(x => x.Context).Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

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
                    .Handle(new GetIfAccountWithNameExistsQuery(name), default);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}