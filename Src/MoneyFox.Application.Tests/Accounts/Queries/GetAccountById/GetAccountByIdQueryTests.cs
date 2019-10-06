using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Accounts.Queries.GetAccountById;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries.GetAccountById
{
    [ExcludeFromCodeCoverage ]
    public class GetAccountByIdQueryTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetAccountByIdQueryTests()
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
            var account2 = new Account("test3", 80);
            await context.AddAsync(account1);
            await context.AddAsync(account2);
            await context.SaveChangesAsync();

            // Act
            Account result = await new GetAccountByIdQuery.Handler(context).Handle(new GetAccountByIdQuery(account1.Id), default);

            // Assert
            result.Name.ShouldEqual(account1.Name);
        }
    }
}
