namespace MoneyFox.Core.Tests.Features.BudgetCreation;

using MoneyFox.Core.Features.BudgetCreation;
using MoneyFox.Domain.Tests.TestFramework;
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
            Name: testData.Name,
            SpendingLimit: testData.SpendingLimit,
            BudgetInterval: testData.Interval,
            Categories: testData.Categories);

        await handler.Handle(command: query, cancellationToken: CancellationToken.None);

        // Assert
        var loadedBudget = Context.Budgets.First();
        AssertBudget(actual: loadedBudget, expected: testData);
    }
}
