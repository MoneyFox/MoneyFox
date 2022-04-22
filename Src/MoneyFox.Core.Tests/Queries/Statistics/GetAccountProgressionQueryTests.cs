namespace MoneyFox.Core.Tests.Queries.Statistics
{

    using System;
    using Common.Exceptions;
    using Core.Queries.Statistics;
    using FluentAssertions;
    using Xunit;

    public class GetAccountProgressionQueryTests
    {
        [Fact]
        public void ExceptionOnInvalidDates()
        {
            // Act / Assert
            // Arrange
            Assert.Throws<StartAfterEnddateException>(
                () => new GetAccountProgressionQuery(accountId: 0, startDate: DateTime.Today.AddYears(3), endDate: DateTime.Today));
        }

        [Fact]
        public void NoExceptionOnSameDate()
        {
            // Arrange
            var date = DateTime.Now;

            // Act
            var query = new GetAccountProgressionQuery(accountId: 0, startDate: date, endDate: date);

            // Assert
            query.Should().NotBeNull();
        }
    }

}
