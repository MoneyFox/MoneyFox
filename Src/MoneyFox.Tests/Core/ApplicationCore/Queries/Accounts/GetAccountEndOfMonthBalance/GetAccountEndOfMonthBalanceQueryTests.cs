namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Accounts.GetAccountEndOfMonthBalance
{

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Queries;
    using MoneyFox.Core.Common;
    using MoneyFox.Core.Common.Helpers;
    using MoneyFox.Core.Common.Interfaces;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using TestFramework;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class GetAccountEndOfMonthBalanceQueryTests
    {
        private readonly AppDbContext context;
        private readonly GetAccountEndOfMonthBalanceQuery.Handler handler;
        private readonly ISystemDateHelper systemDateHelper;

        public GetAccountEndOfMonthBalanceQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            systemDateHelper = Substitute.For<ISystemDateHelper>();
            handler = new GetAccountEndOfMonthBalanceQuery.Handler(context, systemDateHelper: systemDateHelper);
        }

        [Fact]
        public async Task GetCorrectSumForSingleAccount()
        {
            // Arrange
            systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
            var account1 = new Account(name: "test", initialBalance: 100);
            var account2 = new Account(name: "test", initialBalance: 200);
            var payment1 = new Payment(date: new DateTime(year: 2020, month: 09, day: 15), amount: 50, type: PaymentType.Income, chargedAccount: account1);
            var payment2 = new Payment(date: new DateTime(year: 2020, month: 09, day: 25), amount: 50, type: PaymentType.Expense, chargedAccount: account2);
            await context.AddAsync(account1);
            await context.AddAsync(account2);
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(
                request: new GetAccountEndOfMonthBalanceQuery(account1.Id),
                cancellationToken: default);

            // Assert
            result.Should().Be(150);
        }

        [Fact]
        public async Task GetCorrectSumForWithDeactivatedAccount()
        {
            // Arrange
            systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
            var account1 = new Account(name: "test", initialBalance: 100);
            var account2 = new Account(name: "test", initialBalance: 200);
            var account3 = new Account(name: "test", initialBalance: 200);
            var payment1 = new Payment(date: new DateTime(year: 2020, month: 09, day: 15), amount: 50, type: PaymentType.Income, chargedAccount: account1);
            var payment2 = new Payment(date: new DateTime(year: 2020, month: 09, day: 25), amount: 50, type: PaymentType.Expense, chargedAccount: account2);
            account3.Deactivate();
            await context.AddAsync(account1);
            await context.AddAsync(account2);
            await context.AddAsync(account3);
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.SaveChangesAsync();

            // Act
            var result = await handler.Handle(
                request: new GetAccountEndOfMonthBalanceQuery(account1.Id),
                cancellationToken: default);

            // Assert
            result.Should().Be(150);
        }
    }

}
