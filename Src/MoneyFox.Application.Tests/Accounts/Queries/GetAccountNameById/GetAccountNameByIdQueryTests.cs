using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Queries.GetAccountNameById;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries.GetAccountNameById
{
    [ExcludeFromCodeCoverage]
    public class GetAccountNameByIdQueryTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetAccountNameByIdQueryTests()
        {
            context = TestEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetAccountByIdQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account1 = new Account("test2", 80);
            await context.AddAsync(account1);
            await context.SaveChangesAsync();

            // Act
            string result = await new GetAccountNameByIdQuery.Handler(context).Handle(new GetAccountNameByIdQuery(account1.Id), default);

            // Assert
            result.ShouldEqual(account1.Name);
        }
    }
}
