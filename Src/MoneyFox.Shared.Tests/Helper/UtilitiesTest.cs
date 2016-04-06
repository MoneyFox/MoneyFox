using System;
using System.Collections.Generic;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Model;
using Xunit;

namespace MoneyFox.Shared.Tests.Helper
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
            statisticItems[0].Value.ShouldBe(3.23);
            statisticItems[1].Value.ShouldBe(6.59);
            statisticItems[2].Value.ShouldBe(55.39);
            statisticItems[3].Value.ShouldBe(9);
        }

        [Fact]
        public void GetEndOfMonth_NoneInput_LastDayOfMonth()
        {
            Utilities.GetEndOfMonth().ShouldBeInstanceOf(typeof(DateTime));
        }

        [Theory]
        [InlineData(6000000.45)]
        [InlineData(6000000)]
        public void FormatLargeNumbers_AmountShort_ValidString(double amount)
        {
            Utilities.FormatLargeNumbers(amount).ShouldBe(amount.ToString("N"));
        }
    }
}