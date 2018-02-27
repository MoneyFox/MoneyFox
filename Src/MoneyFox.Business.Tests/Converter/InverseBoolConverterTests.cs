using MoneyFox.Business.Converter;
using Should;
using Xunit;

namespace MoneyFox.Business.Tests.Converter
{
    public class InverseBoolConverterTests
    {
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void Convert(bool input, bool expectedValue)
        {
            new InverseBoolConverter().Convert(input, null, null, null).ShouldEqual(expectedValue);
        }
    }
}
