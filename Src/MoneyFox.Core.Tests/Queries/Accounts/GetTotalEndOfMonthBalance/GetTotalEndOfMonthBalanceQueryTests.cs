namespace MoneyFox.Core.Tests.Queries.Accounts.GetTotalEndOfMonthBalance
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using ApplicationCore.Domain.Aggregates.AccountAggregate;
    using ApplicationCore.Queries;
    using Common;
    using Common.Interfaces;
    using Core._Pending_;
    using FluentAssertions;
    using Infrastructure;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetTotalEndOfMonthBalanceQueryTests : IDisposable
    {
        private readonly AppDbContext context;
        private readonly IContextAdapter contextAdapterMock;

        public GetTotalEndOfMonthBalanceQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
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
            InMemoryAppDbContextFactory.Destroy(context);
        }

        [Fact]
        public async Task GetIncludedAccountBalanceSummary_CorrectSum()
        {
            // Arrange
            var systemDateHelper = Substitute.For<ISystemDateHelper>();
            systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
            var accountIncluded = new Account(name: "test", initalBalance: 100);
            var payment = new Payment(
                date: new DateTime(year: 2020, month: 09, day: 25),
                amount: 50,
                type: PaymentType.Expense,
                chargedAccount: accountIncluded);

            await context.AddAsync(accountIncluded);
            await context.AddAsync(payment);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetTotalEndOfMonthBalanceQuery.Handler(contextAdapter: contextAdapterMock, systemDateHelper: systemDateHelper).Handle(
                request: new GetTotalEndOfMonthBalanceQuery(),
                cancellationToken: default);

            // Assert
            result.Should().Be(50);
        }

        [Fact]
        public async Task DontIncludeDeactivatedAccountsInBalance()
        {
            // Arrange
            var systemDateHelper = Substitute.For<ISystemDateHelper>();
            systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
            var accountIncluded = new Account(name: "test", initalBalance: 100);
            var accountDeactivated = new Account(name: "test", initalBalance: 100);
            accountDeactivated.Deactivate();
            var payment = new Payment(
                date: new DateTime(year: 2020, month: 09, day: 25),
                amount: 50,
                type: PaymentType.Expense,
                chargedAccount: accountIncluded);

            await context.AddAsync(accountIncluded);
            await context.AddAsync(accountDeactivated);
            await context.AddAsync(payment);
            await context.SaveChangesAsync();

            // Act
            var result = await new GetTotalEndOfMonthBalanceQuery.Handler(contextAdapter: contextAdapterMock, systemDateHelper: systemDateHelper).Handle(
                request: new GetTotalEndOfMonthBalanceQuery(),
                cancellationToken: default);

            // Assert
            result.Should().Be(50);
        }
    }

}
