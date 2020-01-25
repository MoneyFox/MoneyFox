using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Queries.GetAccountNameById;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries.GetAccountNameById
{
    [ExcludeFromCodeCoverage]
    public class GetAccountNameByIdQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly Mock<IContextAdapter> contextAdapterMock;

        public GetAccountNameByIdQueryTests()
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

        protected virtual void Dispose(bool disposing)
        {
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetAccountByIdQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account1 = new Account("test2", 80);
            await context.AddAsync(account1);
            await context.SaveChangesAsync();

            // Act
            string result =
                await new GetAccountNameByIdQuery.Handler(contextAdapterMock.Object).Handle(new GetAccountNameByIdQuery(account1.Id),
                                                                                            default);

            // Assert
            result.ShouldEqual(account1.Name);
        }
    }
}
