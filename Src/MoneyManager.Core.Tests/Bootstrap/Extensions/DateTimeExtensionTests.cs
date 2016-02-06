using System;
using MoneyManager.Core.Extensions;
using Xunit;
using XunitShouldExtension;

namespace MoneyManager.Core.Tests.Extensions
{
    public class DateTimeExtensionTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(12)]
        [InlineData(6)]
        public void GetFirstDayOfMonth_DifferentMonths_DateTimeFirstDay(int month)
        {
            new DateTime(2015, month, 26).GetFirstDayOfMonth().ShouldBe(new DateTime(2015, month, 1));
        }

        [Theory]
        [InlineData(2, 28)]
        [InlineData(12, 31)]
        [InlineData(6, 30)]
        public void GetLastDayOfMonth_DifferentMonths_DateTimeLastDay(int month, int expectedDay)
        {
            new DateTime(2015, month, 26).GetLastDayOfMonth().ShouldBe(new DateTime(2015, month, expectedDay));
        }
    }
}