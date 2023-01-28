namespace MoneyFox.Core.Tests.ApplicationCore.UseCases;

using Core.ApplicationCore.UseCases.BudgetCreation;
using TestFramework;
using static TestFramework.BudgetAssertion;

public sealed class CreateBudgetShould : InMemoryTestBase
{
    private readonly CreateBudget.Handler handler;

    public CreateBudgetShould()
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
