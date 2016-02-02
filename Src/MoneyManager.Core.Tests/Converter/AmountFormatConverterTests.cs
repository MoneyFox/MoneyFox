using MoneyManager.Core.Converter;
using Xunit;
using XunitShouldExtension;

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
            var converter = new AmountFormatConverter();
            var result = converter.ConvertBack(30, null, null, null);

            result.ShouldBe(30);
            converter.ShouldNotBeNull();
        }
    }
}