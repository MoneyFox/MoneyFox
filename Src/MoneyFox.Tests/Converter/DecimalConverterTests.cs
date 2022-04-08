namespace MoneyFox.Tests.Converter
{
    using Core._Pending_;
    using FluentAssertions;
    using MoneyFox.Converter;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using Core.Common;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DecimalConverterTests
    {
        [Theory]
        [InlineData("nl", "36,41", 36.41)]
        [InlineData("en-GB", "34.41", 34.41)]
        [InlineData("de-CH", "33.41", 33.41)]
        [InlineData("de-DE", "33,41", 33.41)]
        public void ConvertCorrectly(string culture, string value, decimal expectedResult)
        {
            // Arrange
            CultureHelper.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = CultureHelper.CurrentCulture;
            var converter = new DecimalConverter();

            // Act
            decimal result = (decimal)converter.ConvertBack(value, null, null, Thread.CurrentThread.CurrentUICulture);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}