using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Manager;
using MoneyManager.Business.Services;

namespace MoneyManager.Business.WindowsPhone.Test.Manager {
    [TestClass]
    public class CurrencyManagerTest {
        [TestMethod]
        [TestCategory("Integration")]
        public async Task CurrencyManager_GetSupportedCountries_Integration() {
            var currencyManager = new CurrencyManager(new JsonService());
            var list = await currencyManager.GetSupportedCountries();

            Assert.IsTrue(list.Any(x => x.ID == "CH"));
            Assert.IsTrue(list.Any(x => x.ID == "US"));
        }

        [TestMethod]
        [Ignore]
        public void CurrencyManager_GetSupportedCountries() {
            
        }        
        
        [TestMethod]
        [Ignore]
        public void CurrencyManager_GetCurrencyRatio() {
            
        }
    }
}
