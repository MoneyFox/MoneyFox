using FluentAssertions;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Queries.Accounts.GetAccountById;
using MoneyFox.Infrastructure.Persistence;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries.GetAccountById
{
    [ExcludeFromCodeCoverage]
    public class GetAccountByIdQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetAccountByIdQueryTests()
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

        [Fact]
        public async Task GetAccountByIdQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account1 = new Account("test2", 80);
            var account2 = new Account("test3", 80);
            await context.AddAsync(account1);
            await context.AddAsync(account2);
            await context.SaveChangesAsync();

            // Act
            Account result =
                await new GetAccountByIdQuery.Handler(contextAdapterMock.Object).Handle(
                    new GetAccountByIdQuery(account1.Id),
                    default);

            // Assert
            result.Name.Should().Be(account1.Name);
        }
    }
}