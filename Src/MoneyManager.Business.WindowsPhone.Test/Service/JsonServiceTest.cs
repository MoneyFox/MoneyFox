using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Services;

namespace MoneyManager.Business.WindowsPhone.Test.Service
{
    [TestClass]
    public class JsonServiceTest
    {
        private const string CURRENCY_SERVICE_URL =
            "http://www.freecurrencyconverterapi.com/api/convert?q={0}&compact=y";

        private const string COUNTRIES_SERVICE_URL = "http://www.freecurrencyconverterapi.com/api/v2/countries";

        [TestMethod]
        [TestCategory("Integration")]
        public async Task JsonService_GetJsonFromService_WrongUrl()
        {
            var jsonService = new JsonService();

            Assert.AreEqual(string.Empty, await jsonService.GetJsonFromService("This is a fake URL"));
        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task JsonService_GetJsonFromService_Countries()
        {
            var jsonService = new JsonService();
            var result = await jsonService.GetJsonFromService(COUNTRIES_SERVICE_URL);

            Assert.IsTrue(result.Contains("CHF"));
            Assert.IsTrue(result.Contains("USD"));
        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task JsonService_GetJsonFromService_Currency()
        {
            const string expected = "{\"CHF-USD\":";

            var jsonService = new JsonService();
            var currencies = string.Format(CURRENCY_SERVICE_URL, "chf-usd");
            var result = await jsonService.GetJsonFromService(currencies);


            Assert.IsTrue(result.Contains(expected));
        }
    }
}