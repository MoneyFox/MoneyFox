using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Services;

namespace MoneyManager.Business.WindowsPhone.Test.Service
{
    [TestClass]
    public class JsonServiceTest
    {
        [TestMethod]
        [TestCategory("Integration")]
        public async Task JsonService_GetJsonFromService_WrongUrl()
        {
            var jsonService = new JsonService();

            Assert.AreEqual(string.Empty, await jsonService.GetJsonFromService("This is a fake URL"));
        }
    }
}