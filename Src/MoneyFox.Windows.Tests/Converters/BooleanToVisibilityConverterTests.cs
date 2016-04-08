using Windows.UI.Xaml;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyFox.Windows.Converter;

namespace MoneyFox.Windows.Tests.Converters
{
    [TestClass]
    public class BooleanToVisibilityConverterTests
    {
        [TestMethod]
        public void Convert_FalseNoParam_Visibility()
        {
            Assert.AreEqual(Visibility.Collapsed, new BooleanToVisibilityConverter().Convert(false, null, null, string.Empty));
        }

        [TestMethod]
        public void Convert_TrueNoParam_Visibility()
        {
            Assert.AreEqual(Visibility.Visible, new BooleanToVisibilityConverter().Convert(true, null, null, string.Empty));
        }

        [TestMethod]
        public void Convert_TrueParam_Visibility()
        {
            Assert.AreEqual(Visibility.Collapsed, new BooleanToVisibilityConverter().Convert(true, null, "revert", string.Empty));
        }

        [TestMethod]
        public void Convert_FalseParam_Visibility()
        {
            Assert.AreEqual(Visibility.Visible, new BooleanToVisibilityConverter().Convert(false, null, "revert", string.Empty));
        }

        [TestMethod]
        public void Convert_FalseFalseParam_Visibility()
        {
            Assert.AreEqual(Visibility.Collapsed, new BooleanToVisibilityConverter().Convert(false, null, "foo", string.Empty));
        }

        [TestMethod]
        public void ConvertBack_falseNoParam_Visibility()
        {
            Assert.AreEqual(Visibility.Visible, new BooleanToVisibilityConverter().ConvertBack(false, null, null, string.Empty));
        }

        [TestMethod]
        public void ConvertBack_FalseParam_Visibility()
        {
            Assert.AreEqual(Visibility.Visible, new BooleanToVisibilityConverter().ConvertBack(false, null, "foo", string.Empty));
        }

        [TestMethod]
        public void ConvertBack_TrueNoParam_Visibility()
        {
            Assert.AreEqual(Visibility.Collapsed, new BooleanToVisibilityConverter().ConvertBack(true, null, null, string.Empty));
        }
    }
}