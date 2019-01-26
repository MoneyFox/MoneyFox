using System;
using System.Globalization;
using System.Threading;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.Utilities
{
    public class UtilitiesTest
    {
        [Fact]
        public void GetEndOfMonth_NoneInput_LastDayOfMonth()
        {
            ServiceLayer.Utilities.Utilities.GetEndOfMonth().ShouldBeType(typeof(DateTime));
        }

        [Theory]
        [InlineData("10'000", "10'000", "de-CH")]
        [InlineData("10000", "10000", "de-CH")]
        [InlineData("10'000.50", "10000.50", "de-CH")]
        [InlineData("0.05", "0.05", "de-CH")]
        [InlineData("10'000", "10'000", "de-DE")]
        [InlineData("10000", "10000", "de-DE")]
        [InlineData("10'000.50", "10000,50", "de-DE")]
        [InlineData("0.05", "0,05", "de-DE")]
        [InlineData("10'000", "10'000", "en-US")]
        [InlineData("10000", "10000", "en-US")]
        [InlineData("10'000.50", "10000.50", "en-US")]
        [InlineData("0.05", "0.05", "en-US")]
        public void RemoveGroupingSeparators(string amount, string expectedResult, string culture)
        {
            // Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture, false);

            // Act / Assert
            Assert.Equal(expectedResult, ServiceLayer.Utilities.Utilities.RemoveGroupingSeparators(amount));
        }
    }
}