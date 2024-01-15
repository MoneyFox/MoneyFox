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
        handler = new(Context, new SystemDateHelper());
    }

    [Fact]
    public async Task LoadPaymentsForBudget()
    {
        // Arrange
        var category = new TestData.CategoryBeverages();
        var dbCategory = Context.RegisterCategory(category);
        var expense = new TestData.ClearedExpense { Date = DateTime.Today};
        var outEarlierThanRange = new TestData.ClearedExpense { Date = DateTime.Today.AddMonths(-1)};
        var outLaterThanRange = new TestData.ClearedExpense { Date = DateTime.Today.AddMonths(1)};
        Context.RegisterPayment(expense, dbCategory);
        Context.RegisterPayment(outEarlierThanRange, dbCategory);
        Context.RegisterPayment(outLaterThanRange, dbCategory);
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
        entry.AccountName.Should().Be(expense.ChargedAccount.Name);
        entry.Category.Should().Be(category.Name);
        entry.IsCleared.Should().BeTrue();
        entry.IsRecurring.Should().BeFalse();
    }
}
