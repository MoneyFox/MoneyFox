using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.Models;
using MoneyFox.Foundation.Tests;

namespace MoneyFox.Shared.Tests.Helper
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
            statisticItems[0].Value.ShouldBe(3.23);
            statisticItems[1].Value.ShouldBe(6.59);
            statisticItems[2].Value.ShouldBe(55.39);
            statisticItems[3].Value.ShouldBe(9);
        }

        [TestMethod]
        public void GetEndOfMonth_NoneInput_LastDayOfMonth()
        {
            Utilities.GetEndOfMonth().ShouldBeInstanceOf(typeof(DateTime));
        }

        [TestMethod]
        public void FormatLargeNumbers_NumberWithFloat_ValidString()
        {
            const double amount = 6000000.45;
            Utilities.FormatLargeNumbers(amount).ShouldBe(amount.ToString("N"));
        }

        [TestMethod]
        public void FormatLargeNumbers_NumberWithoutFloat_ValidString()
        {
            const double amount = 6000000;
            Utilities.FormatLargeNumbers(amount).ShouldBe(amount.ToString("N"));
        }
    }
}