using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.Application.Common.Extensions;
using Xunit;

namespace MoneyFox.Application.Tests.Common.Extensions
{
    [ExcludeFromCodeCoverage]
    public class DateTimeExtensionTests
    {
        [Fact]
        public void GetFirstDayOfMonth()
        {
            Assert.Equal(new DateTime(2017, 03, 01), new DateTime(2017, 03, 29).GetFirstDayOfMonth());
        }

        [Theory]
        [InlineData(02, 28)]
        [InlineData(01, 31)]
        [InlineData(04, 30)]
        public void GetLastDayOfMonth(int month, int expectedDay)
        {
            Assert.Equal(new DateTime(2017, month, expectedDay), new DateTime(2017, month, 15).GetLastDayOfMonth());
        }
    }
}
