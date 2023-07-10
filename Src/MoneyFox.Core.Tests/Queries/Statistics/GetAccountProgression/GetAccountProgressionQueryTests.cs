namespace MoneyFox.Core.Tests.Queries.Statistics.GetAccountProgression;

using Core.Queries.Statistics;
using FluentAssertions;

public class GetAccountProgressionQueryTests
{
    [Fact]
    public void ExceptionOnInvalidDates()
    {
        // Act / Assert
        // Arrange
        Assert.Throws<InvalidDateRangeException>(
            () => new GetAccountProgression.Query(accountId: 0, startDate: DateTime.Today.AddYears(3), endDate: DateTime.Today));
    }

    [Fact]
    public void NoExceptionOnSameDate()
    {
        // Arrange
        var date = DateTime.Now;

        // Act
        var query = new GetAccountProgression.Query(accountId: 0, startDate: date, endDate: date);

        // Assert
        query.Should().NotBeNull();
    }
}
