using Windows.UI.Xaml;
using MoneyFox.Windows.Converter;
using MvvmCross.Platform.UI;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.Windows.Tests.Converter
{
    public class BooleanToVisibilityConverterTests
    {
        [Theory]
        [InlineData(false, null, Visibility.Collapsed)]
        [InlineData(true, null, Visibility.Visible)]
        [InlineData(true, "revert", Visibility.Collapsed)]
        [InlineData(false, "revert", Visibility.Visible)]
        [InlineData(false, "foo", Visibility.Collapsed)]
        public void Convert_Bool_Visibility(bool input, object parameter, MvxVisibility result)
        {
            new BooleanToVisibilityConverter().Convert(input, null, parameter, string.Empty)
                .ShouldBe(result);
        }

        [Theory]
        [InlineData(false, null, Visibility.Visible)]
        [InlineData(true, null, Visibility.Collapsed)]
        [InlineData(false, "foo", Visibility.Visible)]
        public void ConvertBack_Bool_Visibility(bool input, object parameter, MvxVisibility result)
        {
            new BooleanToVisibilityConverter().ConvertBack(input, null, parameter, string.Empty)
                .ShouldBe(result);
        }
    }
}