namespace MoneyFox.Tests.Core.ApplicationCore.Queries.Statistics
{

    using System;
    using FluentAssertions;
    using MoneyFox.Core.ApplicationCore.Domain.Exceptions;
    using MoneyFox.Core.ApplicationCore.Queries.Statistics;
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
