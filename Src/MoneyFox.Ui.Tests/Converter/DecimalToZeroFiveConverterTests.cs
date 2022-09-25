namespace MoneyFox.Ui.Tests.Converter;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using MoneyFox.Core.Common.Helpers;
using MoneyFox.Ui.Converter;
using Xunit;

[ExcludeFromCodeCoverage]
public class DecimalToZeroFiveConverterTests
{
    [Theory]
    [InlineData("nl", 36.41, "36.40")]
    [InlineData("en-GB", 36.41, "36.40")]
    [InlineData("de-CH", 36.41, "36.40")]
    [InlineData("de-DE", 36.41, "36.40")]
    [InlineData("nl", 36.43, "36.45")]
    [InlineData("en-GB", 36.43, "36.45")]
    [InlineData("de-CH", 36.43, "36.45")]
    [InlineData("de-DE", 36.43, "36.45")]
    public void ConvertCorrectly(string culture, decimal value, string expectedResult)
    {
        // Arrange
        var converter = new DecimalToZeroFiveConverter();

        // Act
        string result = (string)converter.Convert(value: value, targetType: null, parameter: null, culture: new CultureInfo(culture));

        // Assert
        _ = result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("nl", "36,41", 36.41)]
    [InlineData("en-GB", "34.41", 34.41)]
    [InlineData("de-CH", "33.41", 33.41)]
    [InlineData("de-DE", "33,41", 33.41)]
    public void ConvertCorrectlyBack(string culture, string value, decimal expectedResult)
    {
        // Arrange
        CultureHelper.CurrentCulture = new CultureInfo(culture);
        Thread.CurrentThread.CurrentUICulture = CultureHelper.CurrentCulture;
        var converter = new DecimalToZeroFiveConverter();

        // Act
        decimal result = (decimal)converter.ConvertBack(value: value, targetType: null, parameter: null, culture: Thread.CurrentThread.CurrentUICulture);

        // Assert
        _ = result.Should().Be(expectedResult);
    }
}

