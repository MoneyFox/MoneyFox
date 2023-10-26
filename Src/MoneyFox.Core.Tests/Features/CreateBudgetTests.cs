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
    public async Task AddBudgetToDb()
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
        var dbBudget = Context.Budgets.First();
        AssertBudget(actual: dbBudget, expected: testData);
    }
}
