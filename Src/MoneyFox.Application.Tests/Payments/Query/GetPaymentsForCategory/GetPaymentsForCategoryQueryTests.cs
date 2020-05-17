using FluentAssertions;
using MoneyFox.Application.Payments.Queries.GetPaymentsForCategory;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Application.Tests.Payments.Query.GetPaymentsForCategory
{
    [ExcludeFromCodeCoverage]
    public class GetPaymentsForCategoryQueryTests
    {
        [Fact]
        public void AssignCorrectlyInCtor()
        {
            // Arrange
            const int catId = 5234;
            var dateRangeFrom = DateTime.Now.AddDays(1);
            var dateRangeTo = DateTime.Now.AddDays(2);

            // Act
            var query = new GetPaymentsForCategoryQuery(catId, dateRangeFrom, dateRangeTo);

            // Assert
            query.CategoryId.Should().Be(catId);
            query.DateRangeFrom.Should().Be(dateRangeFrom);
            query.DateRangeTo.Should().Be(dateRangeTo);
        }
    }
}
