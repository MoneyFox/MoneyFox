namespace MoneyFox.Core.Tests.Domain.Aggregates.BudgetAggregate;

using Core.ApplicationCore.Domain.Aggregates.BudgetAggregate;
using Core.ApplicationCore.Domain.Exceptions;
using FluentAssertions;

public sealed class SpendingLimitShould
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
