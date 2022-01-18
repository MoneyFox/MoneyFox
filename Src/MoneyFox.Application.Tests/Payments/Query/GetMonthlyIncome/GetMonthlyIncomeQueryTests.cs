using FluentAssertions;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Payments.Queries.GetMonthlyIncome;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Core;
using MoneyFox.Core.Aggregates.Accounts;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Infrastructure.Persistence;
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
        private readonly IContextAdapter contextAdapter;

        public GetMonthlyIncomeQueryTests()
        {
            context = InMemoryEfCoreContextFactory.Create();

            contextAdapter = Substitute.For<IContextAdapter>();
            contextAdapter.Context.Returns(context);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

        [Fact]
        public async Task ReturnCorrectAmount()
        {
            // Arrange
            var systemDateHelper = Substitute.For<ISystemDateHelper>();
            systemDateHelper.Today.Returns(new DateTime(2020, 09, 05));

            var account = new Account("test", 80);

            var payment1 = new Payment(new DateTime(2020, 09, 10), 50, PaymentType.Income, account);
            var payment2 = new Payment(new DateTime(2020, 09, 18), 20, PaymentType.Income, account);
            var payment3 = new Payment(new DateTime(2020, 09, 4), 30, PaymentType.Expense, account);

            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.AddAsync(payment3);
            await context.SaveChangesAsync();

            // Act
            decimal sum =
                await new GetMonthlyIncomeQuery.Handler(contextAdapter, systemDateHelper).Handle(
                    new GetMonthlyIncomeQuery(),
                    default);

            // Assert
            sum.Should().Be(70);
        }
    }
}