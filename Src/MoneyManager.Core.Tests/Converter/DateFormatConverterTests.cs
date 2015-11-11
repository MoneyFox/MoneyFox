using System;
using MoneyManager.Core.Converter;
using MoneyManager.TestFoundation;
using Xunit;

namespace MoneyManager.Core.Tests.Converter
{
    public class DateFormatConverterTests
    {
        [Fact]
        public void Convert_DateTime_ValidString()
        {
            var date = new DateTime(2015, 09, 15, 14, 56, 48);

            new DateTimeFormatConverter()
                .Convert(date, null, null, null)
                .ShouldBe(date.ToString("D"));
        }

        [Fact]
        public void ConvertBack_DateTime_ValidString()
        {
            var date = new DateTime(2015, 09, 15, 14, 56, 48);

            new DateTimeFormatConverter()
                .ConvertBack(date.ToString("D"), null, null, null)
                .ShouldBe(date.ToString("d"));
        }
    }
}