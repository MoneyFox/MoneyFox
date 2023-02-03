namespace MoneyFox.Core.Tests.Features;

using Core.Features.BudgetCreation;
using Domain.Tests.TestFramework;
using static Domain.Tests.TestFramework.BudgetAssertion;

public sealed class CreateBudgetTests : InMemoryTestBase
{
    private readonly CreateBudget.Handler handler;

    public CreateBudgetTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task AddBudgetToRepository()
    {
        // Arrange
        var testData = new TestData.DefaultBudget();

        // Act
        var query = new CreateBudget.Command(
            name: testData.Name,
            spendingLimit: testData.SpendingLimit,
            budgetTimeRange: testData.BudgetTimeRange,
            categories: testData.Categories);

        await handler.Handle(request: query, cancellationToken: CancellationToken.None);

        // Assert
        var loadedBudget = Context.Budgets.First();
        AssertBudget(actual: loadedBudget, expected: testData);
    }
}
