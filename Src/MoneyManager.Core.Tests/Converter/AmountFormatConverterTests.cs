using MoneyManager.Core.Converter;
using NUnit.Framework;
using Shouldly;

namespace MoneyManager.Core.Tests.Converter
{
    [TestFixture]
    public class AmountFormatConverterTests
    {
        public void Convert_FloatAmount_ValidString()
        {
            var amount = 123.45;
            new AmountFormatConverter().Convert(amount, null, null, null).ShouldBe(amount.ToString("C"));
        }

        [Test]
        public void ConvertBack_Input_EqualsInput()
        {
            var converter = new AmountFormatConverter();
            var result = converter.ConvertBack(30, null, null, null);

            result.ShouldBe(30);
            converter.ShouldNotBeNull();
        }
    }
}