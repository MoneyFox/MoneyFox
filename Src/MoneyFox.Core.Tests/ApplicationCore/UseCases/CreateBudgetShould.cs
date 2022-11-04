namespace MoneyFox.Core.Tests.ApplicationCore.UseCases;

using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using Core.ApplicationCore.UseCases.BudgetCreation;
using FluentAssertions;
using NSubstitute;
using TestFramework;
using static TestFramework.BudgetAssertion;

public sealed class CreateBudgetShould
{
    private readonly IBudgetRepository budgetRepository;
    private readonly CreateBudget.Handler handler;

    public CreateBudgetShould()
    {
        budgetRepository = Substitute.For<IBudgetRepository>();
        handler = new(budgetRepository);
    }

    [Fact]
    public async Task AddBudgetToRepository()
    {
        // Capture
        Budget? capturedBudget = null;
        await budgetRepository.AddAsync(Arg.Do<Budget>(b => capturedBudget = b));

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
        capturedBudget.Should().NotBeNull();
        AssertBudget(actual: capturedBudget!, expected: testData);
    }
}
