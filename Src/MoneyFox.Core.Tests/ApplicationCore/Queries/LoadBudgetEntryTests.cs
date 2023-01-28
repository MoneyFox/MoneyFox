namespace MoneyFox.Core.Tests.ApplicationCore.Queries;

using System.Collections.Immutable;
using Core.ApplicationCore.Queries.BudgetEntryLoading;
using FluentAssertions;
using Infrastructure.Persistence;
using TestFramework;

public class LoadBudgetEntryTests
{
    private readonly AppDbContext appDbContext;
    private readonly LoadBudgetEntry.Handler handler;

    protected LoadBudgetEntryTests()
    {
        appDbContext = InMemoryAppDbContextFactory.Create();
        handler = new(appDbContext);
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
            var dbCategory = appDbContext.RegisterCategory(testCategory);
            var testBudget = new TestData.DefaultBudget { Categories = ImmutableList.Create(dbCategory.Id) };
            var dbBudget = appDbContext.RegisterBudget(testBudget);

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
