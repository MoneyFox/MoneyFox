using System;
using MoneyFox.Business.Converter;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.Converter
{
    public class DateFormatConverterTests
    {
        [Fact]
        public void Convert_DateTime_ValidString()
        {
            var date = new DateTime(2015, 09, 15, 14, 56, 48);
            new DateTimeFormatConverter().Convert(date, null, null, null).ShouldEqual(date.ToString("D"));
        }

        [Fact]
        public void ConvertBack_DateTime_ValidString()
        {
            var date = new DateTime(2015, 09, 15, 14, 56, 48);
            new DateTimeFormatConverter().ConvertBack(date, null, null, null).ShouldEqual(date.ToString("d"));
        }
    }
}