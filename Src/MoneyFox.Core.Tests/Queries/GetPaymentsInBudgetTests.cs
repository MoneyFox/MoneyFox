namespace MoneyFox.Core.Tests.Queries;

using System.Collections.Immutable;
using Core.Common;
using Core.Queries;
using Domain.Tests.TestFramework;

public sealed class GetPaymentsInBudgetTests : InMemoryTestBase
{
    private readonly GetPaymentsInBudget.Handler handler;

    public GetPaymentsInBudgetTests()
    {
        handler = new(dbContext: Context, systemDateHelper: new SystemDateHelper());
    }

    [Fact]
    public async Task LoadPaymentsInTimeRange()
    {
        // Arrange
        var category = new TestData.CategoryBeverages();
        var dbCategory = Context.RegisterCategory(category);
        var expense = new TestData.ClearedExpense { Date = DateTime.Today };
        var outEarlierThanRange = new TestData.ClearedExpense { Date = DateTime.Today.AddMonths(-1) };
        var outLaterThanRange = new TestData.ClearedExpense { Date = DateTime.Today.AddMonths(1) };
        Context.RegisterPayment(testPayment: expense, category: dbCategory);
        Context.RegisterPayment(testPayment: outEarlierThanRange, category: dbCategory);
        Context.RegisterPayment(testPayment: outLaterThanRange, category: dbCategory);
        var budget = new TestData.DefaultBudget { Interval = new(1), Categories = ImmutableList.Create(expense.Category!.Id) };
        Context.RegisterBudget(budget);

        // Act
        var query = new GetPaymentsInBudget.Query(new(budget.Id));
        var result = await handler.Handle(query: query, cancellationToken: default);

        // Assert
        result.Should().ContainSingle();
        var entry = result.Single();
        entry.PaymentId.Should().Be(expense.Id);
        entry.Amount.Should().Be(expense.Amount);
        entry.Account.Should().Be(expense.ChargedAccount.Name);
        entry.Category.Should().Be(category.Name);
        entry.IsCleared.Should().BeTrue();
        entry.IsRecurring.Should().BeFalse();
    }

    [Fact]
    public async Task OnlyLoadPaymentsWithCorrectCategories()
    {
        // Arrange
        var expense = new TestData.ClearedExpense { Date = DateTime.Today };
        var expenseWithDifferentCategory = new TestData.ClearedExpense { Date = DateTime.Today };
        var expenseWithoutCategory = new TestData.ClearedExpense { Category = null, Date = DateTime.Today };
        Context.RegisterPayments(expense, expenseWithDifferentCategory, expenseWithoutCategory);
        var budget = new TestData.DefaultBudget { Interval = new(1), Categories = ImmutableList.Create(expense.Category!.Id) };
        Context.RegisterBudget(budget);

        // Act
        var query = new GetPaymentsInBudget.Query(new(budget.Id));
        var result = await handler.Handle(query: query, cancellationToken: default);

        // Assert
        result.Should().ContainSingle();
        var entry = result.Single();
        entry.PaymentId.Should().Be(expense.Id);
        entry.Amount.Should().Be(expense.Amount);
        entry.Account.Should().Be(expense.ChargedAccount.Name);
        entry.Category.Should().Be(expense.Category.Name);
        entry.IsCleared.Should().BeTrue();
        entry.IsRecurring.Should().BeFalse();
    }
}
