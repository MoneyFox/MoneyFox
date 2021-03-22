using FluentAssertions;
using MoneyFox.Application;
using MoneyFox.Converter;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using Xunit;

namespace MoneyFox.Tests.Converter
{
    [ExcludeFromCodeCoverage]
    public class DecimalConverterTests
    {
        [Theory]
        [InlineData("en-US","nl", "36,41", 36.41)]
        [InlineData("en-GB","en-GB", "34.41", 34.41)]
        [InlineData("de-CH","de-CH", "33.41", 33.41)]
        [InlineData("de-DE","de-DE", "33,41", 33.41)]
        public void ConvertCorrectly(string culture, string locale, string value, decimal expectedResult)
        {
            // Arrange
            CultureHelper.CurrentCulture = new CultureInfo(culture);
            CultureHelper.CurrentLocale = new CultureInfo(locale);
            Thread.CurrentThread.CurrentUICulture = CultureHelper.CurrentCulture;
            var converter = new DecimalConverter();

            // Act
            decimal result = (decimal) converter.ConvertBack(value, null, null, Thread.CurrentThread.CurrentUICulture);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
