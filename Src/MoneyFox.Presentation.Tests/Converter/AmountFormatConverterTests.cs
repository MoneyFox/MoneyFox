using MoneyFox.Application;
using MoneyFox.Presentation.Converter;
using Should;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;

namespace MoneyFox.Presentation.Tests.Converter
{
    [ExcludeFromCodeCoverage]
    [Collection("CultureCollection")]
    public class AmountFormatConverterTests
    {
        [Theory] // Currencies: 
        [InlineData("fr-Fr")] // France
        [InlineData("de-DE")] // Germany
        [InlineData("de-CH")] // Switzerland
        [InlineData("en-US")] // United States
        [InlineData("en-GB")] // United Kingdom
        [InlineData("it-IT")] // Italian
        public void Convert_NegativeAndDifferentCurrency_FloatAmount_ValidString(string cultureId)
        {
            CultureHelper.CurrentCulture = new CultureInfo(cultureId, false);
            decimal amount = -88.23m;

            new AmountFormatConverter()
               .Convert(amount, null, null, CultureInfo.CurrentCulture)
               .ShouldEqual(amount.ToString("C", new CultureInfo(cultureId, false)));

            CultureHelper.CurrentCulture = CultureInfo.CurrentCulture;
        }
    }
}
