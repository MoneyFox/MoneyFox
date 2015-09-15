using MoneyManager.Core.Converter;
using MoneyManager.Foundation;
using Xunit;

namespace MoneyManager.Core.Tests.Converter
{
    public class AmountFormatConverterTests
    {
        [Theory]
        [InlineData(123.45, "CHF 123.45")]
        [InlineData(123, "CHF 123.00")]
        public void Convert_DateTime_ValidString(double amount, string result)
        {
            new AmountFormatConverter().Convert(amount, null, null, null).ShouldBe(result);
        }
    }
}
