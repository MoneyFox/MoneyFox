using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MoneyFox.Application.Payments.Queries;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using Xunit;

namespace MoneyFox.Application.Tests.Payments
{
    [ExcludeFromCodeCoverage]
    public class GetUnlcearedPaymentsOfThisMonthQueryTests : IDisposable
    {
        private readonly EfCoreContext context;

        public GetUnlcearedPaymentsOfThisMonthQueryTests()
        {
            context = TestEfCoreContextFactory.Create();
        }
        public void Dispose()
        {
            TestEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetPaymentsQuery_CorrectNumberLoaded()
        {
            // Arrange
            var account = new Account("test", 80);

            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);
            var payment2 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);
            var payment3 = new Payment(DateTime.Now.AddMonths(1), 20, PaymentType.Expense, account);

            payment2.ClearPayment();

            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.AddAsync(payment3);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetUnclearedPaymentsOfThisMonthQuery.Handler(context).Handle(new GetUnclearedPaymentsOfThisMonthQuery(), default);

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GetPaymentsQuery_WithAccountId_CorrectNumberLoaded()
        {
            // Arrange
            var account1 = new Account("test", 80);
            var account2 = new Account("test", 80);

            var payment1 = new Payment(DateTime.Now, 20, PaymentType.Expense, account1);
            var payment2 = new Payment(DateTime.Now, 20, PaymentType.Expense, account2);

            payment1.ClearPayment();
            payment2.ClearPayment();

            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetUnclearedPaymentsOfThisMonthQuery.Handler(context).Handle(new GetUnclearedPaymentsOfThisMonthQuery{AccountId = account1.Id}, default);

            // Assert
            Assert.Single(result);
        }
    }
}
