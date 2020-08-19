using FluentAssertions;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Queries.GetMonthlyIncome;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Query.GetMonthlyIncome
{
    [ExcludeFromCodeCoverage]
    public class GetMonthlyIncomeQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly IContextAdapter contextAdapterMock;

        public GetMonthlyIncomeQueryTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapterMock = Substitute.For<IContextAdapter>();
            contextAdapterMock.Context.Returns(context);
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
        public async Task ReturnCorrectAmount()
        {
            // Arrange
            var account = new Account("test", 80);

            var payment1 = new Payment(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 10), 50, PaymentType.Income, account);
            var payment2 = new Payment(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 18), 20, PaymentType.Income, account);
            var payment3 = new Payment(DateTime.Now, 30, PaymentType.Expense, account);

            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.AddAsync(payment3);
            await context.SaveChangesAsync();

            // Act
            var sum = await new GetMonthlyIncomeQuery.Handler(contextAdapterMock).Handle(new GetMonthlyIncomeQuery(), default);

            // Assert
            sum.Should().Be(70);
        }
    }
}
