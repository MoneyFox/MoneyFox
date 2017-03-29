using System;
using MoneyFox.Business.Extensions;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MoneyFox.Business.Tests.Extensions
{
    public class DateTimeExtensionTests
    {
        [Fact]
        public void GetFirstDayOfMonth()
        {
            Assert.AreEqual(new DateTime(2017, 03, 01), new DateTime(2017, 03, 29).GetFirstDayOfMonth());
        }

        [Theory]
        [InlineData(02, 28)]
        [InlineData(01, 31)]
        [InlineData(04, 30)]
        public void GetLastDayOfMonth(int month, int expectedDay)
        {
            Assert.AreEqual(new DateTime(2017, month, expectedDay), new DateTime(2017, month, 15).GetLastDayOfMonth());
        }
    }
}
