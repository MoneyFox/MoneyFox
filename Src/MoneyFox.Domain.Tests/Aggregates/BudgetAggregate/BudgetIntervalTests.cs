namespace MoneyFox.Domain.Tests.Aggregates.BudgetAggregate;

using Domain.Aggregates.BudgetAggregate;
using Exceptions;
using FluentAssertions;

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

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void ThrowWhenInvalidNumberOfMonthsPassed(int numberOfMonths)
    {
        // Act
        Action act = () => _ = new BudgetInterval(numberOfMonths);

        // Assert
        act.Should().Throw<InvalidArgumentException>();
    }
}
