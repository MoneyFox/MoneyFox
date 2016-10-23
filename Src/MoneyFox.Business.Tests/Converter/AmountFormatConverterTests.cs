using MoneyFox.Business.Converter;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.Business.Tests.Converter
{
    public class AmountFormatConverterTests
    {
        [Fact]
        public void Convert_FloatAmount_ValidString()
        {
            var amount = 123.45;
            new AmountFormatConverter().Convert(amount, null, null, null).ShouldBe(amount.ToString("C"));
        }

        [Fact]
        public void ConvertBack_Input_EqualsInput()
        {
            var amount = 30;
            new AmountFormatConverter().Convert(amount, null, null, null).ShouldBe(amount.ToString("C"));
        }
    }
}