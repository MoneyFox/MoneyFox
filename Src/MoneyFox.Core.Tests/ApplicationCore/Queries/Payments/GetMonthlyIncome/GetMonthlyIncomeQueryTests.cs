namespace MoneyFox.Core.Tests.ApplicationCore.Queries.Payments.GetMonthlyIncome
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
    public class GetMonthlyIncomeQueryTests
    {
        private readonly AppDbContext context;
        private readonly GetMonthlyIncomeQuery.Handler handler;
        private readonly ISystemDateHelper systemDateHelper;

        public GetMonthlyIncomeQueryTests()
        {
            context = InMemoryAppDbContextFactory.Create();
            systemDateHelper = Substitute.For<ISystemDateHelper>();
            handler = new GetMonthlyIncomeQuery.Handler(appDbContext: context, systemDateHelper: systemDateHelper);
        }

        [Fact]
        public async Task ReturnCorrectAmount()
        {
            // Arrange
            systemDateHelper.Today.Returns(new DateTime(year: 2020, month: 09, day: 05));
            var account = new Account(name: "test", initialBalance: 80);
            var payment1 = new Payment(date: new DateTime(year: 2020, month: 09, day: 10), amount: 50, type: PaymentType.Income, chargedAccount: account);
            var payment2 = new Payment(date: new DateTime(year: 2020, month: 09, day: 18), amount: 20, type: PaymentType.Income, chargedAccount: account);
            var payment3 = new Payment(date: new DateTime(year: 2020, month: 09, day: 4), amount: 30, type: PaymentType.Expense, chargedAccount: account);
            await context.AddAsync(payment1);
            await context.AddAsync(payment2);
            await context.AddAsync(payment3);
            await context.SaveChangesAsync();

            // Act
            var sum = await handler.Handle(request: new GetMonthlyIncomeQuery(), cancellationToken: default);

            // Assert
            sum.Should().Be(70);
        }
    }

}
