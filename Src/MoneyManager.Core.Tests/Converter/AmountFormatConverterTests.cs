using System;
using System.Globalization;
using MoneyManager.Core.Converter;
using MoneyManager.TestFoundation;
using Xunit;

namespace MoneyManager.Core.Tests.Converter
{
    public class AmountFormatConverterTests
    {
        [Theory]
        [InlineData(123.45)]
        [InlineData(123)]
        public void Convert_Amount_ValidString(double amount)
        {
            new AmountFormatConverter().Convert(amount, null, null, null).ShouldBe(amount.ToString("C"));
        }

        [Fact]
        public void ConvertBack_Input_EqualsInput()
        {
            AmountFormatConverter converter = new AmountFormatConverter();
            var result = converter.ConvertBack(30, (Type)null, (object)null, (CultureInfo)null);

            result.ShouldBe(30);
            ((object)converter).ShouldNotBeNull();
        }
    }
}