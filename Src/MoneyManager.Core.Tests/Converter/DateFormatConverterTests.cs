using System;
using MoneyManager.Core.Converter;
using MoneyManager.Foundation;
using Xunit;

namespace MoneyManager.Core.Tests.Converter
{
    public class DateFormatConverterTests
    {
        [Fact]
        public void Convert_DateTime_ValidString()
        {
            new DateTimeFormatConverter().Convert(new DateTime(2015, 09, 15, 14, 56, 48), null, null, null)
                .ShouldBe("Dienstag, 15. September 2015");
        }
    }
}