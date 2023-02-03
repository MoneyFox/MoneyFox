namespace MoneyFox.Domain.Tests.Aggregates.BudgetAggregate;

using Domain.Aggregates.BudgetAggregate;
using Exceptions;
using FluentAssertions;

public sealed class SpendingLimitTests
{
    [Fact]
    public void BeCorrectlyCreated()
    {
        // Arrange
        var val = 10.5m;

        // Act
        var spendingLimit = new SpendingLimit(val);

        // Assert
        spendingLimit.Value.Should().Be(val);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void ThrowWhenInvalidSpendingLimitPassed(decimal val)
    {
        // Act
        Action act = () => _ = new SpendingLimit(val);

        // Assert
        act.Should().Throw<InvalidArgumentException>();
    }
}
