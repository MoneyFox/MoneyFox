namespace MoneyFox.Ui.Tests.Converter;

using System.Diagnostics.CodeAnalysis;
using Core.Common.Helpers;
using FluentAssertions;
using Ui.Converter;
using Xunit;

[Collection("Conversion")]
public class DecimalConverterTests
{
    [Theory]
    [InlineData("nl", 36.41, "36,41")]
    [InlineData("en-GB", 36.41, "36.41")]
    [InlineData("de-CH", 36.41, "36.41")]
    [InlineData("de-DE", 36.41, "36,41")]
    public void ConvertCorrectly(string culture, decimal value, string expectedResult)
    {
        // Arrange
        CultureHelper.CurrentCulture = new(culture);
        Thread.CurrentThread.CurrentUICulture = CultureHelper.CurrentCulture;
        var converter = new DecimalConverter();

        // Act
        var result = (string)converter.Convert(value: value, targetType: null!, parameter: null!, culture: new(culture));

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
        CultureHelper.CurrentCulture = new(culture);
        Thread.CurrentThread.CurrentUICulture = CultureHelper.CurrentCulture;
        var converter = new DecimalConverter();

        // Act
        var result = (decimal)converter.ConvertBack(value: value, targetType: null!, parameter: null!, culture: Thread.CurrentThread.CurrentUICulture);

        // Assert
        _ = result.Should().Be(expectedResult);
    }
}
