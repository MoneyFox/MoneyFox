using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Converter;

namespace MoneyManager.Core.Tests.Converter
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
            new AmountFormatConverter().Convert(amount, null, null, null).ShouldBe(amount.ToString("C"));
        }
    }
}