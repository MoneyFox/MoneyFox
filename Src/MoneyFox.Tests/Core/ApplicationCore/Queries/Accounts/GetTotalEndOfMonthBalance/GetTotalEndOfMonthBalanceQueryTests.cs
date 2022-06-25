namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Accounts.GetTotalEndOfMonthBalance
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Core.Common.Helpers;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetTotalEndOfMonthBalanceQueryTests
    {
        private readonly AppDbContext context;
        private readonly ISystemDateHelper systemDateHelper;
        private readonly GetTotalEndOfMonthBalanceQuery.Handler handler;

        public GetTotalEndOfMonthBalanceQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            systemDateHelper = Substitute.For<ISystemDateHelper>();
            handler = new GetTotalEndOfMonthBalanceQuery.Handler(appDbContext: context, systemDateHelper: systemDateHelper);
        }

        [Fact]
        public async Task GetIncludedAccountBalanceSummary_CorrectSum()
        {
            // Arrange
            systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
            var accountIncluded = new Account(name: "test", initialBalance: 100);
            var payment = new Payment(
                date: new DateTime(year: 2020, month: 09, day: 25),
                amount: 50,
                type: PaymentType.Expense,
                chargedAccount: accountIncluded);

            await context.AddAsync(accountIncluded);
            await context.AddAsync(payment);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(request: new GetTotalEndOfMonthBalanceQuery(), cancellationToken: default);

            // Assert
            result.Should().Be(50);
        }

        [Fact]
        public async Task DontIncludeDeactivatedAccountsInBalance()
        {
            // Arrange
            systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
            var accountIncluded = new Account(name: "test", initialBalance: 100);
            var accountDeactivated = new Account(name: "test", initialBalance: 100);
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
            var result = await handler.Handle(request: new GetTotalEndOfMonthBalanceQuery(), cancellationToken: default);

            // Assert
            result.Should().Be(50);
        }
    }

}
