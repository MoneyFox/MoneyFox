using System;
using System.Collections.Generic;
using System.Linq;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Tests;
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
            result[0].Value.ShouldBe(3.23);
            result[1].Value.ShouldBe(6.59);
            result[2].Value.ShouldBe(55.39);
            result[3].Value.ShouldBe(9);
        }

        [Fact]
        public void GetEndOfMonth_NoneInput_LastDayOfMonth()
        {
            Utilities.GetEndOfMonth().ShouldBeInstanceOf(typeof(DateTime));
        }

        [Theory]
        [InlineData(6000000.45)]
        [InlineData(6000000)]
        [InlineData(6000000.4567)]
        public void FormatLargeNumbers_ValidString(double amount)
        {
            Utilities.FormatLargeNumbers(amount).ShouldBe(amount.ToString("N"));
        }
    }
}