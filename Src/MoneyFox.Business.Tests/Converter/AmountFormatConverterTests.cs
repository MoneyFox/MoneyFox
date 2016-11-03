using MoneyFox.Business.Converter;
using System;
using System.Globalization;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.Business.Tests.Converter
{
    public class AmountFormatConverterTests
    {
        [Theory]
        [InlineData(123.45)]
        [InlineData(30)]
        public void Convert_FloatAmount_ValidString(double amount)
        {
            new AmountFormatConverter().Convert(amount, null, null, null).ShouldBe(amount.ToString("C"));
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