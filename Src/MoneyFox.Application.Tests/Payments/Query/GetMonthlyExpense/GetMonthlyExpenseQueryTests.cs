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
    public class GetMonthlyExpenseQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly IContextAdapter contextAdapterMock;

        public GetMonthlyExpenseQueryTests()
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

            var payment1 = new Payment(DateTime.Now.AddDays(-2), 50, PaymentType.Expense, account);
            var payment2 = new Payment(DateTime.Now, 20, PaymentType.Expense, account);
            var payment3 = new Payment(DateTime.Now, 30, PaymentType.Income, account);

            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.AddAsync(payment3);
            await context.SaveChangesAsync();

            // Act
            var sum = await new GetMonthlyExpenseQuery.Handler(contextAdapterMock).Handle(new GetMonthlyExpenseQuery(), default);

            // Assert
            sum.Should().Be(70);
        }
    }
}
