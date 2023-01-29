namespace MoneyFox.Core.Tests.ApplicationCore.Queries;

using System.Collections.Immutable;
using Core.ApplicationCore.Queries.BudgetEntryLoading;
using FluentAssertions;

public class LoadBudgetEntryTests : InMemoryTestBase
{
    private readonly LoadBudgetEntry.Handler handler;

    protected LoadBudgetEntryTests()
    {
        handler = new(Context);
    }

    public class GivenNoBudgets : LoadBudgetEntryTests
    {
        [Fact]
        public void ThrowException()
        {
            // Act
            var query = new LoadBudgetEntry.Query(10);
            Func<Task> act = () => handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<InvalidOperationException>();
        }
    }

    public class GivenBudgetsExist : LoadBudgetEntryTests
    {
        [Fact]
        public async Task ReturnBudgetWithCorrectId()
        {
            // Arrange
            var testCategory = new TestData.DefaultCategory();
            var dbCategory = Context.RegisterCategory(testCategory);
            var testBudget = new TestData.DefaultBudget { Categories = ImmutableList.Create(dbCategory.Id) };
            var dbBudget = Context.RegisterBudget(testBudget);

            // Act
            var query = new LoadBudgetEntry.Query(dbBudget.Id.Value);
            var budgetEntryData = await handler.Handle(request: query, cancellationToken: CancellationToken.None);

            // Assert
            budgetEntryData.Id.Should().Be(dbBudget.Id);
            budgetEntryData.Name.Should().Be(dbBudget.Name);
            budgetEntryData.SpendingLimit.Should().Be(dbBudget.SpendingLimit);
            budgetEntryData.Categories.Should()
                .BeEquivalentTo(testBudget.Categories.Select(id => new BudgetEntryData.BudgetCategory(id: id, name: testCategory.Name)));
        }
    }
}
