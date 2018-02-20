using Windows.UI.Xaml;
using MoneyFox.Windows.Converter;
using Should;
using Xunit;

namespace MoneyFox.Windows.Tests.Converters
{
    public class BooleanToVisibilityConverterTests
    {
        [Theory]
        [InlineData(false, null, Visibility.Collapsed)]
        [InlineData(true, null, Visibility.Visible)]
        [InlineData(true, "revert", Visibility.Collapsed)]
        [InlineData(false, "revert", Visibility.Visible)]
        [InlineData(false, "foo", Visibility.Collapsed)]
        [InlineData(true, "foo", Visibility.Visible)]
        public void Convert_Param_CorrectVisibility(bool value, string param, Visibility expectedResult)
        {
            new BooleanToVisibilityConverter().Convert(value, null, param, string.Empty).ShouldBeSameAs(expectedResult);
        }
    }
}