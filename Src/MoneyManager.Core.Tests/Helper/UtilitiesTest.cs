using System;
using System.Collections.Generic;
using System.Globalization;
using MoneyManager.Core.Helper;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using Xunit;

namespace MoneyManager.Core.Tests.Helper
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
            Utilities.RoundStatisticItems(statisticItems);

            Assert.Equal(statisticItems[0].Value, 3.23);
            Assert.Equal(statisticItems[1].Value, 6.59);
            Assert.Equal(statisticItems[2].Value, 55.39);
            Assert.Equal(statisticItems[3].Value, 9);
        }

        [Fact]
        public void GetEndOfMonth_NoneInput_LastDayOfMonth()
        {
            Utilities.GetEndOfMonth().ShouldBeInstanceOf<DateTime>();
        }

        [Theory]
        [InlineData(6000000.45, "de-DE", "6.000.000,45")]
        [InlineData(6000000, "de-DE", "6.000.000,00")]
        [InlineData(6000000, "en-US", "6,000,000.00")]
        public void FormatLargeNumbers_AmountShort_ValidString(double amount, string culture, string result)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            Utilities.FormatLargeNumbers(amount).ShouldBe(result);
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;
        }
    }
}