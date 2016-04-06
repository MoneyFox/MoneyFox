using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Converter;

namespace MoneyFox.Shared.Tests.Converter
{
    [TestClass]
    public class DateFormatConverterTests
    {
        [TestMethod]
        public void Convert_DateTime_ValidString()
        {
            var date = new DateTime(2015, 09, 15, 14, 56, 48);

            Assert.AreEqual(date.ToString("D"), new DateTimeFormatConverter()
                .Convert(date, null, null, null));
        }

        [TestMethod]
        public void ConvertBack_DateTime_ValidString()
        {
            var date = new DateTime(2015, 09, 15, 14, 56, 48);

            Assert.AreEqual(date.ToString("D"), new DateTimeFormatConverter()
                .Convert(date, null, null, null));
        }
    }
}