using FluentAssertions;
using MoneyFox.Core._Pending_;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Queries.Accounts.GetTotalEndOfMonthBalance;
using MoneyFox.Core.Tests.Infrastructure;
using MoneyFox.Infrastructure.Persistence;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Core.Tests.Queries.Accounts.GetTotalEndOfMonthBalance
{
    [ExcludeFromCodeCoverage]
    public class GetTotalEndOfMonthBalanceQueryTests : IDisposable
    {
        private readonly EfCoreContext context;
        private readonly IContextAdapter contextAdapterMock;

        public GetTotalEndOfMonthBalanceQueryTests()
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
        public async Task GetIncludedAccountBalanceSummary_CorrectSum()
        {
            // Arrange
            var systemDateHelper = Substitute.For<ISystemDateHelper>();
            systemDateHelper.Today.Returns(new DateTime(2020, 09, 05));

            var accountIncluded = new Account("test", 100);
            var payment = new Payment(new DateTime(2020, 09, 25), 50, PaymentType.Expense, accountIncluded);

            await context.AddAsync(accountIncluded);
            await context.AddAsync(payment);
            await context.SaveChangesAsync();

            // Act
            decimal result =
                await new GetTotalEndOfMonthBalanceQuery.Handler(contextAdapterMock, systemDateHelper).Handle(
                    new GetTotalEndOfMonthBalanceQuery(),
                    default);

            // Assert
            result.Should().Be(50);
        }

        [Fact]
        public async Task DontIncludeDeactivatedAccountsInBalance()
        {
            // Arrange
            var systemDateHelper = Substitute.For<ISystemDateHelper>();
            systemDateHelper.Today.Returns(new DateTime(2020, 09, 05));

            var accountIncluded = new Account("test", 100);
            var accountDeactivated = new Account("test", 100);
            accountDeactivated.Deactivate();

            var payment = new Payment(new DateTime(2020, 09, 25), 50, PaymentType.Expense, accountIncluded);

            await context.AddAsync(accountIncluded);
            await context.AddAsync(accountDeactivated);
            await context.AddAsync(payment);
            await context.SaveChangesAsync();

            // Act
            decimal result =
                await new GetTotalEndOfMonthBalanceQuery.Handler(contextAdapterMock, systemDateHelper).Handle(
                    new GetTotalEndOfMonthBalanceQuery(),
                    default);

            // Assert
            result.Should().Be(50);
        }
    }
}