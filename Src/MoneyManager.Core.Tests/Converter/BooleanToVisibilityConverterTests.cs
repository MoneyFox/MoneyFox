using System.Globalization;
using MoneyManager.Core.Converter;
using MoneyManager.TestFoundation;
using MvvmCross.Platform.UI;
using Xunit;

namespace MoneyManager.Core.Tests.Converter
{
    public class BooleanToVisibilityConverterTests
    {
        [Theory]
        [InlineData(false, null, MvxVisibility.Collapsed)]
        [InlineData(true, null, MvxVisibility.Visible)]
        [InlineData(true, "revert", MvxVisibility.Collapsed)]
        [InlineData(false, "revert", MvxVisibility.Visible)]
        [InlineData(false, "foo", MvxVisibility.Collapsed)]
        public void Convert_Bool_Visibility(bool input, object parameter, MvxVisibility result)
        {
            new BooleanToVisibilityConverter().Convert(input, null, parameter, CultureInfo.CurrentCulture)
                .ShouldBe(result);
        }

        [Theory]
        [InlineData(false, null, MvxVisibility.Visible)]
        [InlineData(true, null, MvxVisibility.Collapsed)]
        [InlineData(false, "foo", MvxVisibility.Visible)]
        public void ConvertBack_Bool_Visibility(bool input, object parameter, MvxVisibility result)
        {
            new BooleanToVisibilityConverter().ConvertBack(input, null, parameter, CultureInfo.CurrentCulture)
                .ShouldBe(result);
        }
    }
}