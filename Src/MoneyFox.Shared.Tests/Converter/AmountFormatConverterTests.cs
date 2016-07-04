using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Converter;

namespace MoneyFox.Shared.Tests.Converter
{
    [TestClass]
    public class AmountFormatConverterTests
    {
        [TestMethod]
        public void Convert_FloatAmount_ValidString()
        {
            var amount = 123.45;
            Assert.AreEqual(amount.ToString("C"), new AmountFormatConverter().Convert(amount, null, null, null));
        }

        [TestMethod]
        public void ConvertBack_Input_EqualsInput()
        {
            var amount = 30;
            Assert.AreEqual(amount.ToString("C"), new AmountFormatConverter().Convert(amount, null, null, null));
        }
    }
}