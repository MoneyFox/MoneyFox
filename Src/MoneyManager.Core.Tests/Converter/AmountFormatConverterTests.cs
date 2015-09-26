using MoneyManager.Core.Converter;
using MoneyManager.Foundation;
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
    }
}