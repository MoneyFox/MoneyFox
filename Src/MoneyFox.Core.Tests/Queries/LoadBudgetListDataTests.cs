namespace MoneyFox.Core.Tests.Queries;

using System.Collections.Immutable;
using Core.Common;
using Core.Queries.BudgetList;
using Domain.Aggregates.AccountAggregate;
using Domain.Tests.TestFramework;
using FluentAssertions;
using NSubstitute;

public sealed class LoadBudgetListDataTests : InMemoryTestBase
{
    private readonly LoadBudgetListData.Handler handler;
    private readonly ISystemDateHelper systemDateHelper;

    public LoadBudgetListDataTests()
    {
        systemDateHelper = Substitute.For<ISystemDateHelper>();
        systemDateHelper.Today.Returns(DateTime.Today);
        systemDateHelper.Now.Returns(DateTime.Now);
        handler = new(systemDateHelper: systemDateHelper, appDbContext: Context);
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

    [Theory]
    [InlineData(6)]
    [InlineData(12)]
    [InlineData(24)]
    public async Task ReturnBudgets_WithCorrectSummarizedSpending_WithMonthsWithoutPaymentsInBetween(int numberOfMonths)
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
            Date = now.AddMonths(-(numberOfMonths + 1))
        };

        var dbPayment1 = Context.RegisterPayment(testExpense1);
        var dbPayment2 = Context.RegisterPayment(testExpense2);
        var dbPayment3 = Context.RegisterPayment(testExpense3);
        var testBudget = new TestData.DefaultBudget
        {
            Interval = new(numberOfMonths),
            Categories = ImmutableList.Create(dbPayment1.CategoryId!.Value, dbPayment2.CategoryId!.Value, dbPayment3.CategoryId!.Value)
        };

        Context.RegisterBudget(testBudget);

        // Act
        var query = new LoadBudgetListData.Query();
        var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        var budgetListData = result.Single();
        var expectedCurrentSpending = testExpense1.Amount + testExpense2.Amount;
        AssertBudgetListData(actualBudgetListData: budgetListData, expectedBudgetTestData: testBudget, expectedCurrentSpending: expectedCurrentSpending);
    }

    [Fact]
    public async Task ReturnBudgets_WithCorrectSummarizedSpending_SingleMonthInterval()
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
            Date = now.AddMonths(-1)
        };

        var dbPayment1 = Context.RegisterPayment(testExpense1);
        var dbPayment2 = Context.RegisterPayment(testExpense2);
        var dbPayment3 = Context.RegisterPayment(testExpense3);
        var testBudget = new TestData.DefaultBudget
        {
            Interval = new(1), Categories = ImmutableList.Create(dbPayment1.CategoryId!.Value, dbPayment2.CategoryId!.Value, dbPayment3.CategoryId!.Value)
        };

        Context.RegisterBudget(testBudget);

        // Act
        var query = new LoadBudgetListData.Query();
        var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        var budgetListData = result.Single();
        var expectedCurrentSpending = testExpense1.Amount + testExpense2.Amount;
        AssertBudgetListData(actualBudgetListData: budgetListData, expectedBudgetTestData: testBudget, expectedCurrentSpending: expectedCurrentSpending);
    }

    [Fact]
    public async Task ReturnBudgets_WithCurrentSpendingSetToZero_WhenItWouldBeNegative()
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
            Type = PaymentType.Income,
            Date = now.Date
        };

        var dbPayment1 = Context.RegisterPayment(testExpense1);
        var testBudget = new TestData.DefaultBudget { Interval = new(1), Categories = ImmutableList.Create(dbPayment1.CategoryId!.Value) };
        Context.RegisterBudget(testBudget);

        // Act
        var query = new LoadBudgetListData.Query();
        var result = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().HaveCount(1);
        var budgetListData = result.Single();
        var expectedCurrentSpending = 0;
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

        var dbPayment1 = Context.RegisterPayment(testExpense1);
        var testBudget1 = new TestData.DefaultBudget { Categories = ImmutableList<int>.Empty };
        var testBudget2 = new TestData.DefaultBudget { Categories = ImmutableList.Create(dbPayment1.CategoryId!.Value) };
        Context.RegisterBudgets(testBudget1, testBudget2);

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
