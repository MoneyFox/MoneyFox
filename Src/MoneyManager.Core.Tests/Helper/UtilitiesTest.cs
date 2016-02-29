using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Assert = Xunit.Assert;

namespace MoneyManager.Core.Tests.Helper
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
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

        [TestMethod]
        public void GetEndOfMonth_NoneInput_LastDayOfMonth()
        {
            Utilities.GetEndOfMonth().ShouldBeInstanceOf(typeof (DateTime));
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