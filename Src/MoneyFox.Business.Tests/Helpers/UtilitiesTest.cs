using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.Models;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.Helpers
{
    public class UtilitiesTest
    {
        [Fact]
        public void RoundStatisticItems_ListOfItems_ListWithRoundedItems()
        {
            var statisticItems = new List<StatisticItem>
            {
                new StatisticItem
                {
                    Value = 3.234
                },
                new StatisticItem
                {
                    Value = 6.589
                },
                new StatisticItem
                {
                    Value = 55.385
                },
                new StatisticItem
                {
                    Value = 9
                }
            };
            var result = Utilities.RoundStatisticItems(statisticItems).ToList();
            result[0].Value.ShouldEqual(3.23);
            result[1].Value.ShouldEqual(6.59);
            result[2].Value.ShouldEqual(55.39);
            result[3].Value.ShouldEqual(9);
        }

        [Fact]
        public void GetEndOfMonth_NoneInput_LastDayOfMonth()
        {
            Utilities.GetEndOfMonth().ShouldBeType(typeof(DateTime));
        }

        [Theory]
        [InlineData(6000000.45)]
        [InlineData(6000000)]
        [InlineData(6000000.4567)]
        public void FormatLargeNumbers_ValidString(double amount)
        {
            Utilities.FormatLargeNumbers(amount).ShouldEqual(amount.ToString("N"));
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
            Assert.Equal(expectedResult, Utilities.RemoveGroupingSeparators(amount));
        }
    }
}