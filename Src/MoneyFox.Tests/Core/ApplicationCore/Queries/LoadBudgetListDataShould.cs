namespace MoneyFox.Tests.Core.ApplicationCore.Queries
{

    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
    using MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
    using MoneyFox.Core.ApplicationCore.Queries.BudgetListLoading;
    using MoneyFox.Core.Common.Helpers;
    using MoneyFox.Infrastructure.Persistence;
    using NSubstitute;
    using TestFramework;
    using Xunit;

    public sealed class LoadBudgetListDataShould
    {
        private readonly ISystemDateHelper systemDateHelper;
        private readonly AppDbContext dbContext;
        private readonly LoadBudgetListData.Handler handler;

        public LoadBudgetListDataShould()
        {
            systemDateHelper = Substitute.For<ISystemDateHelper>();
            systemDateHelper.Today.Returns(DateTime.Today);
            systemDateHelper.Now.Returns(DateTime.Now);
            dbContext = InMemoryAppDbContextFactory.Create();
            handler = new LoadBudgetListData.Handler(systemDateHelper: systemDateHelper, appDbContext: dbContext);
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
                Date = DateTime.Today
            };

            var testExpense2 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 60.3m,
                Type = PaymentType.Income,
                Date = DateTime.Today
            };

            var testTransfer = new TestData.DefaultExpense
            {
                Id = 11,
                Amount = 60.3m,
                Type = PaymentType.Transfer,
                Date = DateTime.Today
            };

            var dbPayment1 = dbContext.RegisterPayment(testExpense1);
            var dbPayment2 = dbContext.RegisterPayment(testExpense2);
            var dbPayment3 = dbContext.RegisterPayment(testTransfer);
            var testBudget = new TestData.DefaultBudget
            {
                Categories = ImmutableList.Create(dbPayment1.CategoryId!.Value, dbPayment2.CategoryId!.Value, dbPayment3.CategoryId!.Value)
            };

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
        public async Task ReturnBudgets_WithCorrectSummarizedSpending_ForPaymentsInCurrentYear_WithMonthsWithoutPaymentsInBetween()
        {
            // Arrange
            systemDateHelper.Now.Returns(
                new DateTime(
                    year: DateTime.Today.Year,
                    month: 10,
                    day: 12,
                    hour: 11,
                    minute: 52,
                    second: 0));

            var testExpense1 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 100m,
                Type = PaymentType.Expense,
                Date = new DateTime(year: DateTime.Today.Year, month: 1, day: 1)
            };

            var testExpense2 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 50m,
                Type = PaymentType.Expense,
                Date = new DateTime(year: DateTime.Today.Year, month: 1, day: 1)
            };

            var testExpense3 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 50m,
                Type = PaymentType.Expense,
                Date = DateTime.Now.AddYears(-1)
            };

            var dbPayment1 = dbContext.RegisterPayment(testExpense1);
            var dbPayment2 = dbContext.RegisterPayment(testExpense2);
            var dbPayment3 = dbContext.RegisterPayment(testExpense3);
            var testBudget = new TestData.DefaultBudget
            {
                BudgetTimeRange = BudgetTimeRange.YearToDate,
                Categories = ImmutableList.Create(dbPayment1.CategoryId!.Value, dbPayment2.CategoryId!.Value, dbPayment3.CategoryId!.Value)
            };

            dbContext.RegisterBudget(testBudget);

            // Act
            var query = new LoadBudgetListData.Query();
            var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            var loadedBudget = result.Single();
            AssertBudgetListData(actualBudgetListData: loadedBudget, expectedBudgetTestData: testBudget, expectedCurrentSpending: 15);
        }

        [Theory]
        [InlineData(BudgetTimeRange.Last1Year, 1)]
        public async Task ReturnBudgets_WithCorrectSummarizedSpending_WithMonthsWithoutPaymentsInBetween(BudgetTimeRange timeRange, int yearsToDeduct)
        {
            // Arrange
            var now = new DateTime(
                year: DateTime.Today.Year,
                month: 10,
                day: 12,
                hour: 11,
                minute: 52,
                second: 0);

            systemDateHelper.Now.Returns(now);

            var testExpense1 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 100m,
                Type = PaymentType.Expense,
                Date = now.Date
            };

            var testExpense2 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 50m,
                Type = PaymentType.Expense,
                Date = now.Date
            };

            var testExpense3 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 50m,
                Type = PaymentType.Expense,
                Date = now.AddYears(-(yearsToDeduct + 1))
            };

            var dbPayment1 = dbContext.RegisterPayment(testExpense1);
            var dbPayment2 = dbContext.RegisterPayment(testExpense2);
            var dbPayment3 = dbContext.RegisterPayment(testExpense3);
            var testBudget = new TestData.DefaultBudget
            {
                BudgetTimeRange = timeRange,
                Categories = ImmutableList.Create(dbPayment1.CategoryId!.Value, dbPayment2.CategoryId!.Value, dbPayment3.CategoryId!.Value)
            };

            dbContext.RegisterBudget(testBudget);

            // Act
            var query = new LoadBudgetListData.Query();
            var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            var budgetListData = result.Single();
            var expectedCurrentSpending = (testExpense1.Amount + testExpense2.Amount) / (yearsToDeduct * 12);
            AssertBudgetListData(actualBudgetListData: budgetListData, expectedBudgetTestData: testBudget, expectedCurrentSpending: expectedCurrentSpending);
        }

        [Fact]
        public async Task ReturnAllBudgets_EvenWhenOneBudgetDoesHaveNoPayment()
        {
            // Arrange
            var testExpense1 = new TestData.DefaultExpense
            {
                Id = 10,
                Amount = 100m,
                Type = PaymentType.Expense,
                Date = new DateTime(year: DateTime.Today.Year, month: 1, day: 20)
            };

            var dbPayment1 = dbContext.RegisterPayment(testExpense1);
            var testBudget1 = new TestData.DefaultBudget { Categories = ImmutableList<int>.Empty };
            var testBudget2 = new TestData.DefaultBudget { Categories = ImmutableList.Create(dbPayment1.CategoryId.Value) };
            dbContext.RegisterBudgets(testBudget1, testBudget2);

            // Act
            var query = new LoadBudgetListData.Query();
            var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
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
