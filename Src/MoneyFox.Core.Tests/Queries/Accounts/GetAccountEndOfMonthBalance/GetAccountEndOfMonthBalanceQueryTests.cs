using FluentAssertions;
using MoneyFox.Core._Pending_;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Queries.Accounts.GetAccountEndOfMonthBalance;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Queries.Accounts.GetAccountEndOfMonthBalance
{
    [ExcludeFromCodeCoverage]
    public class GetAccountEndOfMonthBalanceQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly IContextAdapter contextAdapterMock;

        public GetAccountEndOfMonthBalanceQueryTests()
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

        protected virtual void Dispose(bool disposing) => InMemoryEfCoreContextFactory.Destroy(context);

        [Fact]
        public async Task GetCorrectSumForSingleAccount()
        {
            // Arrange
            var systemDateHelper = Substitute.For<ISystemDateHelper>();
            systemDateHelper.Today.Returns(new DateTime(2020, 09, 05));

            var account1 = new Account("test", 100);
            var account2 = new Account("test", 200);
            var payment1 = new Payment(new DateTime(2020, 09, 15), 50, PaymentType.Income, account1);
            var payment2 = new Payment(new DateTime(2020, 09, 25), 50, PaymentType.Expense, account2);

            await context.AddAsync(account1);
            await context.AddAsync(account2);
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.SaveChangesAsync();

            // Act
            decimal result =
                await new GetAccountEndOfMonthBalanceQuery.Handler(contextAdapterMock, systemDateHelper).Handle(
                    new GetAccountEndOfMonthBalanceQuery(account1.Id),
                    default);

            // Assert
            result.Should().Be(150);
        }

        [Fact]
        public async Task GetCorrectSumForWithDeactivatedAccount()
        {
            // Arrange
            var systemDateHelper = Substitute.For<ISystemDateHelper>();
            systemDateHelper.Today.Returns(new DateTime(2020, 09, 05));

            var account1 = new Account("test", 100);
            var account2 = new Account("test", 200);
            var account3 = new Account("test", 200);
            var payment1 = new Payment(new DateTime(2020, 09, 15), 50, PaymentType.Income, account1);
            var payment2 = new Payment(new DateTime(2020, 09, 25), 50, PaymentType.Expense, account2);

            account3.Deactivate();

            await context.AddAsync(account1);
            await context.AddAsync(account2);
            await context.AddAsync(account3);
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.SaveChangesAsync();

            // Act
            decimal result =
                await new GetAccountEndOfMonthBalanceQuery.Handler(contextAdapterMock, systemDateHelper).Handle(
                    new GetAccountEndOfMonthBalanceQuery(account1.Id),
                    default);

            // Assert
            result.Should().Be(150);
        }
    }
}