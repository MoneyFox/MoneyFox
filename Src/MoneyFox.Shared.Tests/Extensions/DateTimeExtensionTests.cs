using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Extensions;

namespace MoneyFox.Shared.Tests.Extensions
{
    [TestClass]
    public class DateTimeExtensionTests
    {
        [TestMethod]
        public void GetFirstDayOfMonth_TwoDigitMonth_DateTimeFirstDay()
        {
            new DateTime(2015, 12, 26).GetFirstDayOfMonth().ShouldBe(new DateTime(2015, 12, 1));
        }

        [TestMethod]
        public void GetFirstDayOfMonth_SingleDigitMonth_DateTimeFirstDay()
        {
            new DateTime(2015, 2, 26).GetFirstDayOfMonth().ShouldBe(new DateTime(2015, 2, 1));
        }

        [TestMethod]
        public void GetLastDayOfMonth_TwoDigitMonth_DateTimeLastDay()
        {
            new DateTime(2015, 12, 26).GetLastDayOfMonth().ShouldBe(new DateTime(2015, 12, 31));
        }

        [TestMethod]
        public void GetLastDayOfMonth_SingleDigitMonth_DateTimeLastDay()
        {
            new DateTime(2015, 1, 26).GetLastDayOfMonth().ShouldBe(new DateTime(2015, 1, 31));
        }

        [TestMethod]
        public void GetLastDayOfMonth_February_DateTimeLastDay()
        {
            new DateTime(2015, 2, 26).GetLastDayOfMonth().ShouldBe(new DateTime(2015, 2, 28));
        }
    }
}