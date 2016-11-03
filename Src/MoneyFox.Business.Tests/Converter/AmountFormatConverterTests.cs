using MoneyFox.Business.Converter;
using System.Globalization;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.Business.Tests.Converter
{
    public class AmountFormatConverterTests
    {
        [Theory]
        [InlineData(123.45, "en-US")]
        [InlineData(30, "en-US")]
        [InlineData(123.45, "de-CH")]
        [InlineData(30, "de-CH")]
        [InlineData(123.45, "it-IT")]
        [InlineData(30, "it-IT")]
        public void Convert_FloatAmount_ValidString(double amount, string cultureString)
        {
            new AmountFormatConverter().Convert(amount, null, null, new CultureInfo(cultureString)).ShouldBe(amount.ToString("C", new CultureInfo(cultureString)));
        }

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
            //var amount = c.ToString(testCulture);
            new AmountFormatConverter().Convert(amount, null, null, testCulture).ShouldBe("-" + testCulture.NumberFormat.CurrencySymbol + positiveAmount.ToString(testCulture));
        }
    }
}