namespace MoneyFox.Ui.Tests.Views.Statistics;

using Core.Queries;
using Core.Tests;
using Domain.Tests.TestFramework;
using FluentAssertions;
using Xunit;

public sealed class GetPaymentsForCategorySummaryTests : InMemoryTestBase
{
    [Fact]
    public async Task GetPaymentsForCategoryId()
    {
        // Arrange
        var expense = new TestData.DefaultExpense{ Date = DateTime.Now };
        var dbExpense = Context.RegisterPayment(expense);

        // Act
        var query = new GetPaymentsForCategorySummary.Query(expense.Category.Id, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));
        var result = await new GetPaymentsForCategorySummary.Handler(Context).Handle(query, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().ContainSingle(p => p.Id == dbExpense.Id);
    }

    [Fact]
    public async Task GetPaymentsWithoutCategory()
    {
        // Arrange
        var expenseWithCategory = new TestData.DefaultExpense{ Date = DateTime.Now };
        var dbExpenseWithCategory = Context.RegisterPayment(expenseWithCategory);

        var expense = new TestData.DefaultExpense{ Date = DateTime.Now, Category = null };
        var dbExpense = Context.RegisterPayment(expense);

        // Act
        var query = new GetPaymentsForCategorySummary.Query(null, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));
        var result = await new GetPaymentsForCategorySummary.Handler(Context).Handle(query, cancellationToken: CancellationToken.None);

        // Assert
        result.Should().ContainSingle(p => p.Id == dbExpense.Id);
    }
}
