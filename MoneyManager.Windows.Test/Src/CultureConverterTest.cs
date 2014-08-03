using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace MoneyManager.Windows.Test.Src
{
    [TestClass]
    public class CultureConverterTest
    {
        [TestMethod]
        public void ConvertTest()
        {
            var date = DateTime.Now;

            var result = new CultureConverter().Convert(date, null, "{0:d}", "de-ch");

            Assert.AreEqual(result, date.ToString("d"));
        }
    }
}