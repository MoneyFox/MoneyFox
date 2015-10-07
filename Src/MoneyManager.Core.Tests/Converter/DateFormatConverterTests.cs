using System;
using System.Globalization;
using MoneyManager.Core.Converter;
using MoneyManager.TestFoundation;
using Xunit;

namespace MoneyManager.Core.Tests.Converter
{
    public class DateFormatConverterTests
    {
        [Theory]
        [InlineData("en-US", "Tuesday, September 15, 2015")]
        [InlineData("de-DE", "Dienstag, 15. September 2015")]
        public void Convert_DateTime_ValidString(string culture, string result)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);

            new DateTimeFormatConverter()
                .Convert(new DateTime(2015, 09, 15, 14, 56, 48), null, null, null)
                .ShouldBe(result);

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;
        }
    }
}