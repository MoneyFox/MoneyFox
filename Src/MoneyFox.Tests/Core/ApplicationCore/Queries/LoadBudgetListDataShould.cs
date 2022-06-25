namespace MoneyFox.Tests.Core.ApplicationCore.Queries
{

    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading;
    using MoneyFox.Core.Common.Helpers;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using TestFramework;
    using Xunit;

    public sealed class LoadBudgetListDataShould
    {
        private readonly AppDbContext dbContext;
        private readonly LoadBudgetListData.Handler handler;
        private readonly ISystemDateHelper dateHelper;

        public LoadBudgetListDataShould()
        {
            dbContext = InMemoryAppDbContextFactory.Create();
            handler = new LoadBudgetListData.Handler(dbContext);
            dateHelper = Substitute.For<ISystemDateHelper>();
            dateHelper.Today.Returns(DateTime.Today);
        }

        [Fact]
        public async Task ReturnEmpty_WhenNoBudgetsCreated()
        {
            // Act
            var query = new LoadBudgetListData.Query();
            var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ReturnBudgets_WithCorrectSummarizedSpending()
        {
            // Arrange
            var testExpense1 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 100.5m,
                Type = PaymentType.Expense,
                Date = DateTime.Now
            };

            var testExpense2 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 60.3m,
                Type = PaymentType.Income,
                Date = DateTime.Now
            };

            var dbPayment1 = dbContext.RegisterPayment(testExpense1);
            var dbPayment2 = dbContext.RegisterPayment(testExpense2);
            var testBudget = new TestData.DefaultBudget {Categories = ImmutableList.Create(dbPayment1.CategoryId.Value, dbPayment2.CategoryId.Value)};
            dbContext.RegisterBudget(testBudget);

            // Act
            var query = new LoadBudgetListData.Query();
            var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            var loadedBudget = result.Single();
            AssertBudgetListData(
                actualBudgetListData: loadedBudget,
                expectedBudgetTestData: testBudget,
                expectedCurrentSpending: testExpense1.Amount - testExpense2.Amount);
        }

        [Fact]
        public async Task ReturnBudgets_WithCorrectSummarizedSpending_ForPaymentsInCurrentYear()
        {
            // Arrange
            dateHelper.Today.Returns(new DateTime(DateTime.Today.Year, 2, 10));
            var testExpense1 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 100m,
                Type = PaymentType.Expense,
                Date = new DateTime(DateTime.Today.Year, 1, 20)
            };

            var testExpense2 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 50m,
                Type = PaymentType.Expense,
                Date = new DateTime(DateTime.Today.Year, 2, 5)
            };

            var dbPayment1 = dbContext.RegisterPayment(testExpense1);
            var dbPayment2 = dbContext.RegisterPayment(testExpense2);
            var testBudget = new TestData.DefaultBudget {Categories = ImmutableList.Create(dbPayment1.CategoryId.Value, dbPayment2.CategoryId.Value)};
            dbContext.RegisterBudget(testBudget);

            // Act
            var query = new LoadBudgetListData.Query();
            var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            var loadedBudget = result.Single();
            var expectedCurrentSpending = (testExpense1.Amount + testExpense2.Amount) / 2;
            AssertBudgetListData(actualBudgetListData: loadedBudget, expectedBudgetTestData: testBudget, expectedCurrentSpending: expectedCurrentSpending);
        }
        [Fact]
        public async Task ReturnBudgets_WithCorrectSummarizedSpending_ForPaymentsInCurrentYear_WithMonthsWithoutPaymentsInBetween()
        {
            // Arrange
            dateHelper.Today.Returns(new DateTime(DateTime.Today.Year, 2, 10));
            var testExpense1 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 100m,
                Type = PaymentType.Expense,
                Date = new DateTime(DateTime.Today.Year, 1, 20)
            };

            var testExpense2 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 50m,
                Type = PaymentType.Expense,
                Date = new DateTime(DateTime.Today.Year, 3, 5)
            };

            var dbPayment1 = dbContext.RegisterPayment(testExpense1);
            var dbPayment2 = dbContext.RegisterPayment(testExpense2);
            var testBudget = new TestData.DefaultBudget {Categories = ImmutableList.Create(dbPayment1.CategoryId.Value, dbPayment2.CategoryId.Value)};
            dbContext.RegisterBudget(testBudget);

            // Act
            var query = new LoadBudgetListData.Query();
            var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            var loadedBudget = result.Single();
            var expectedCurrentSpending = (testExpense1.Amount + testExpense2.Amount) / 3;
            AssertBudgetListData(actualBudgetListData: loadedBudget, expectedBudgetTestData: testBudget, expectedCurrentSpending: expectedCurrentSpending);
        }

        private static void AssertBudgetListData(BudgetListData actualBudgetListData, TestData.IBudget expectedBudgetTestData, decimal expectedCurrentSpending)
        {
            actualBudgetListData.Id.Should().BeGreaterThan(0);
            actualBudgetListData.Name.Should().Be(expectedBudgetTestData.Name);
            actualBudgetListData.SpendingLimit.Should().BeApproximately(expectedValue: expectedBudgetTestData.SpendingLimit, precision: 0.01m);
            actualBudgetListData.CurrentSpending.Should().BeApproximately(expectedValue: expectedCurrentSpending, precision: 0.01m);
        }
    }

}
