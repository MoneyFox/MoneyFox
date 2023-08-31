namespace MoneyFox.Domain.Tests.Aggregates.BudgetAggregate;

using Domain.Aggregates.BudgetAggregate;

public sealed class BudgetIntervalTests
{
    [Fact]
    public void BeCorrectlyCreated()
    {
        // Arrange
        const int numberOfMonths = 10;

        // Act
        var budgetInterval = new BudgetInterval(numberOfMonths);

        // Assert
        budgetInterval.NumberOfMonths.Should().Be(numberOfMonths);
    }
}
