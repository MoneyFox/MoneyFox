namespace MoneyFox.Core.Tests.ApplicationCore.Queries;

using System.Collections.Immutable;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using Core.ApplicationCore.Queries.BudgetListLoading;
using Core.Common.Helpers;
using FluentAssertions;
using Infrastructure.Persistence;
using NSubstitute;
using TestFramework;

public sealed class LoadBudgetListDataTests
{
    private readonly AppDbContext dbContext;
    private readonly LoadBudgetListData.Handler handler;
    private readonly ISystemDateHelper systemDateHelper;

    public LoadBudgetListDataTests()
    {
        systemDateHelper = Substitute.For<ISystemDateHelper>();
        systemDateHelper.Today.Returns(DateTime.Today);
        systemDateHelper.Now.Returns(DateTime.Now);
        dbContext = InMemoryAppDbContextFactory.Create();
        handler = new(systemDateHelper: systemDateHelper, appDbContext: dbContext);
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
    public async Task ReturnBudgets_WithCorrectSummarizedSpending_ForPaymentsInCurrentYear_WithMonthsWithoutPaymentsInBetween()
    {
        // Arrange
        var now = new DateTime(
            year: DateTime.Today.Year,
            month: 11,
            day: 12,
            hour: 11,
            minute: 52,
            second: 0);

        systemDateHelper.Now.Returns(now);
        systemDateHelper.Today.Returns(now.Date);
        var testExpense1 = new TestData.DefaultExpense
        {
            Id = 10,
            Amount = 100m,
            Type = PaymentType.Expense,
            Date = new(year: DateTime.Today.Year, month: 1, day: 1)
        };

        var testExpense2 = new TestData.DefaultExpense
        {
            Id = 10,
            Amount = 50m,
            Type = PaymentType.Expense,
            Date = new(year: DateTime.Today.Year, month: 1, day: 1)
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
    [InlineData(BudgetTimeRange.Last2Years, 2)]
    [InlineData(BudgetTimeRange.Last3Years, 3)]
    [InlineData(BudgetTimeRange.Last5Years, 5)]
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
        systemDateHelper.Today.Returns(now.Date);
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
            Date = new(year: DateTime.Today.Year, month: 1, day: 20)
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
        actualBudgetListData.TimeRange.Should().Be(expectedBudgetTestData.BudgetTimeRange);
        actualBudgetListData.CurrentSpending.Should().BeApproximately(expectedValue: expectedCurrentSpending, precision: 0.01m);
    }
}
