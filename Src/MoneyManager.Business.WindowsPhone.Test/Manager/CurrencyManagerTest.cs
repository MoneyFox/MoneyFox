using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Manager;
using MoneyManager.Business.WindowsPhone.Test.Mocks;

namespace MoneyManager.Business.WindowsPhone.Test.Manager
{
    [TestClass]
    public class CurrencyManagerTest
    {
        [TestMethod]
        public async Task CurrencyManager_GetSupportedCountries()
        {
            var currencyManager = new CurrencyManager(new JsonServiceMock());
            var list = await currencyManager.GetSupportedCountries();

            Assert.IsTrue(list.Any(x => x.ID == "CH"));
            Assert.IsTrue(list.Any(x => x.ID == "US"));
        }
    }
}