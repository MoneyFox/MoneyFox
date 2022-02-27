namespace MoneyFox.Core.Tests.Queries.Statistics
{
    using Core._Pending_.Exceptions;
    using Core.Queries.Statistics.Queries;
    using FluentAssertions;
    using System;
    using Xunit;

    public class GetAccountProgressionQueryTests
    {
        [Fact]
        public void ExceptionOnInvalidDates() =>
            // Arrange
            // Act / Assert
            Assert.Throws<StartAfterEnddateException>(
                () =>
                    new GetAccountProgressionQuery(0, DateTime.Today.AddYears(3), DateTime.Today));

        [Fact]
        public void NoExceptionOnSameDate()
        {
            // Arrange
            DateTime date = DateTime.Now;

            // Act
            var query = new GetAccountProgressionQuery(0, date, date);

            // Assert
            query.Should().NotBeNull();
        }
    }
}