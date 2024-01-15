namespace MoneyFox.Core.Tests.Queries;

using System.Collections.Immutable;
using Core.Queries;
using Domain.Tests.TestFramework;

public sealed class GetPaymentsInBudgetTests : InMemoryTestBase
{
    private readonly GetPaymentsInBudget.Handler handler;

    public GetPaymentsInBudgetTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task LoadPaymentsForBudget()
    {
        // Arrange
        var expense = new TestData.ClearedExpense { Date = DateTime.Today };
        Context.RegisterPayment(expense);
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
        entry.Category.Should().Be(expense.Category.Name);
        entry.IsCleared.Should().BeTrue();
        entry.IsRecurring.Should().BeFalse();
    }
}
