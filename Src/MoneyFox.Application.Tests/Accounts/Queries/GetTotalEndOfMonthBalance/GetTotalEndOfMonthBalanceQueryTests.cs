using FluentAssertions;
using MoneyFox.Application.Accounts.Queries.GetTotalEndOfMonthBalance;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Tests.Infrastructure;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Persistence;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace MoneyFox.Application.Tests.Accounts.Queries.GetTotalEndOfMonthBalance
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

        protected virtual void Dispose(bool disposing)
        {
            InMemoryEfCoreContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetIncludedAccountBalanceSummary_CorrectSum()
        {
            // Arrange
            var accountIncluded = new Account("test", 100);
            var payment = new Payment(DateTime.Now.AddDays(2), 50, PaymentType.Expense, accountIncluded);

            await context.AddAsync(accountIncluded);
            await context.AddAsync(payment);
            await context.SaveChangesAsync();

            // Act
            decimal result = await new GetTotalEndOfMonthBalanceQuery.Handler(contextAdapterMock).Handle(
                new GetTotalEndOfMonthBalanceQuery(), default);

            // Assert
            result.Should().Be(50);
        }

        [Fact]
        public async Task GetCorrectSumForSingleAccount()
        {
            // Arrange
            var account1 = new Account("test", 100);
            var account2 = new Account("test", 200);
            var payment1 = new Payment(DateTime.Now.AddDays(2), 50, PaymentType.Expense, account1);
            var payment2 = new Payment(DateTime.Now.AddDays(2), 50, PaymentType.Expense, account2);

            await context.AddAsync(account1);
            await context.AddAsync(account2);
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.SaveChangesAsync();

            // Act
            decimal result = await new GetTotalEndOfMonthBalanceQuery.Handler(contextAdapterMock).Handle(
                new GetTotalEndOfMonthBalanceQuery(account1.Id), default);

            // Assert
            result.Should().Be(150);
        }
    }
}
