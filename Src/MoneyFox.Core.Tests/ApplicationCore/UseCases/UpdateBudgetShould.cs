namespace MoneyFox.Core.Tests.ApplicationCore.UseCases;

using System.Collections.Immutable;
using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using Core.ApplicationCore.UseCases.BudgetUpdate;
using FluentAssertions;
using NSubstitute;
using TestFramework;
using static TestFramework.BudgetAssertion;

public sealed class UpdateBudgetShould
{
    private readonly IBudgetRepository budgetRepository;
    private readonly UpdateBudget.Handler handler;

    public UpdateBudgetShould()
    {
        budgetRepository = Substitute.For<IBudgetRepository>();
        handler = new(budgetRepository);
    }

    [Fact]
    public async Task PassUpdatedAggregateToRepository()
    {
        // Capture
        var testBudget = new TestData.DefaultBudget();
        budgetRepository.GetAsync(testBudget.Id).Returns(testBudget.CreateDbBudget());
        Budget? capturedBudget = null;
        await budgetRepository.UpdateAsync(Arg.Do<Budget>(b => capturedBudget = b));

        // Arrange
        var updatedBudget = new TestData.DefaultBudget
        {
            Name = "Drinks",
            SpendingLimit = testBudget.SpendingLimit + 11,
            CurrentSpending = testBudget.CurrentSpending + 22,
            Categories = ImmutableList.Create(12, 26)
        };

        // Act
        var command = new UpdateBudget.Command(
            budgetId: testBudget.Id,
            name: updatedBudget.Name,
            spendingLimit: updatedBudget.SpendingLimit,
            budgetTimeRange: updatedBudget.BudgetTimeRange,
            categories: updatedBudget.Categories);

        await handler.Handle(request: command, cancellationToken: CancellationToken.None);

        // Assert
        capturedBudget.Should().NotBeNull();
        AssertBudget(actual: capturedBudget!, expected: updatedBudget);
    }
}



