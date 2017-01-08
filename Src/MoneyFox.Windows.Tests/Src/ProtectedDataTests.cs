using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MoneyFox.Windows.Tests
{
    [TestClass]
    public class ProtectedDataTests
    {
        [TestMethod]
        public void ProtectUnprotect_TestString_CorrectlyWrittenAndRead()
        {
            const string key = "TestKey";
            var value = "TestValue";

            var protectedData = new ProtectedData();
            protectedData.Protect(key, value);

            Assert.AreEqual(value, protectedData.Unprotect(key));
        }

        [TestMethod]
        public void ProtectUnprotect_TestStringSpecialChars_CorrectlyWrittenAndRead()
        {
            const string key = "TestKey";
            var value = "/*ç%\"*%&/";

            var protectedData = new ProtectedData();
            protectedData.Protect(key, value);

            Assert.AreEqual(value, protectedData.Unprotect(key));
        }

        [TestMethod]
        public void ProtectUnprotect_KeySpecialChars_CorrectlyWrittenAndRead()
        {
            const string key = "/*ç%\"*%&/";
            var value = "Fooo";

            var protectedData = new ProtectedData();
            protectedData.Protect(key, value);

            Assert.AreEqual(value, protectedData.Unprotect(key));
        }

        [TestMethod]
        public void ProtectDelete_TestString_CorrectlyWrittenAndDeleted()
        {
            const string key = "TestKey";
            var value = "TestValue";

            var protectedData = new ProtectedData();
            protectedData.Protect(key, value);

            Assert.AreEqual(value, protectedData.Unprotect(key));

            protectedData.Remove(key);
            
            Assert.IsNull(protectedData.Unprotect(key));
        }
    }
}
