using MoneyFox.Presentation.Converter;
using Should;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Presentation.Tests.Converter
{
    [ExcludeFromCodeCoverage]
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
