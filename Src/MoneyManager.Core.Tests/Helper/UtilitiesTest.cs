using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.Helper;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.Tests.Helper
{
    [TestClass]
    public class UtilitiesTest
    {
        [TestMethod]
        public void Utilities_RoundStatisticItems()
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

            Assert.AreEqual(statisticItems[0].Value, 3.23);
            Assert.AreEqual(statisticItems[1].Value, 6.59);
            Assert.AreEqual(statisticItems[2].Value, 55.39);
            Assert.AreEqual(statisticItems[3].Value, 9);
        }

        [TestMethod]
        public void Utilities_GetEndOfMonth()
        {
            Assert.IsInstanceOfType(Utilities.GetEndOfMonth(), typeof (DateTime));
        }
    }
}