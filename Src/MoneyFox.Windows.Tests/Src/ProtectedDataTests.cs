using Xunit;

namespace MoneyFox.Windows.Tests
{
    public class ProtectedDataTests
    {
        [Fact]
        public void ProtectUnprotect_TestString_CorrectlyWrittenAndRead()
        {
            const string key = "TestKey";
            var value = "TestValue";

            var protectedData = new ProtectedData();
            protectedData.Protect(key, value);

            Assert.Equal(value, protectedData.Unprotect(key));
        }

        [Fact]
        public void ProtectUnprotect_TestStringSpecialChars_CorrectlyWrittenAndRead()
        {
            const string key = "TestKey";
            var value = "/*ç%\"*%&/";

            var protectedData = new ProtectedData();
            protectedData.Protect(key, value);

            Assert.Equal(value, protectedData.Unprotect(key));
        }

        [Fact]
        public void ProtectUnprotect_KeySpecialChars_CorrectlyWrittenAndRead()
        {
            const string key = "/*ç%\"*%&/";
            var value = "Fooo";

            var protectedData = new ProtectedData();
            protectedData.Protect(key, value);

            Assert.Equal(value, protectedData.Unprotect(key));
        }

        [Fact]
        public void ProtectDelete_TestString_CorrectlyWrittenAndDeleted()
        {
            const string key = "TestKey";
            var value = "TestValue";

            var protectedData = new ProtectedData();
            protectedData.Protect(key, value);

            Assert.Equal(value, protectedData.Unprotect(key));

            protectedData.Remove(key);
            
            Assert.Null(protectedData.Unprotect(key));
        }
    }
}
