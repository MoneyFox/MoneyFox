using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.Services;

namespace MoneyManager.Business.WindowsPhone.Test.Service {
    [TestClass]
    public class JsonServiceTest {
        [TestMethod]
        [TestCategory("Integration")]
        public void JsonService_GetJsonFromService_WrongUrl() {
            var jsonService = new JsonService();
            
            Assert.AreEqual(String.Empty, jsonService.GetJsonFromService("This is a fake URL"));
        }
    }
}
