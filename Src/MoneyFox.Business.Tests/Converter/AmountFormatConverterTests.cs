using MoneyFox.Business.Converter;
using System.Globalization;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.Converter
{
    public class AmountFormatConverterTests
    {
        [Theory]              // Currencies: 
        [InlineData("fr-Fr")] // France
        [InlineData("de-DE")] // Germany
        [InlineData("de-CH")] // Switzerland
        [InlineData ("en-US")] // United States
        [InlineData("en-GB")] // United Kingdom
        [InlineData("it-IT")] // Italian
        public void Convert_NegativeAndDifferentCurrency_FloatAmount_ValidString(string cultureID)
        {
            CultureInfo testCulture = new CultureInfo(cultureID, false);
            var amount = -88.23;
            var positiveAmount = 88.23;
            new AmountFormatConverter().Convert(amount, null, null, testCulture).ShouldEqual("-" + testCulture.NumberFormat.CurrencySymbol + positiveAmount.ToString(testCulture));
        }
    }
}