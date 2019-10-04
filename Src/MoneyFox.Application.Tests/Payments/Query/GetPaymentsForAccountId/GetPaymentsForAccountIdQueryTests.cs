using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Application.Payments.Queries.GetPaymentsForAccountId;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Should;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Query.GetPaymentsForAccountId
{
    [ExcludeFromCodeCoverage]
    public class GetPaymentsForAccountIdQueryTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetPaymentsForAccountIdQueryTests()
        {
            context = TestEfCoreContextFactory.Create();
        }

        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }
        
        [Fact]
        public async Task GetPaymentsForAccountId_CategoryFound()
        {
            // Arrange
            var account = new Account("test", 80);

            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);
            var payment2 = new Payment(DateTime.Now, 20, PaymentType.Expense, new Account("test", 80));
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetPaymentsForAccountIdQuery.Handler(context).Handle(new GetPaymentsForAccountIdQuery(account.Id), default);

            // Assert
            result.First().Id.ShouldEqual(payment1.Id);
        }
    }
}
