using MoneyFox.Business.Converter;
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

        [Fact]
        public void Convert_NegativeFloatAmount_ValidString()
        {
            var amount = -88.23;
            new AmountFormatConverter().Convert(amount, null, null, System.Globalization.CultureInfo.CurrentCulture).ShouldBe("-$88.23");
        }
    }
}