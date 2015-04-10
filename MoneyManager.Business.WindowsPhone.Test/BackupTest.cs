using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.WindowsPhone.Test.Mocks;

namespace MoneyManager.Business.WindowsPhone.Test {
    [TestClass]
    public class BackupTest {
        [TestMethod]
        public void Backup_GetCreationDateLastBackup() {
            const string expected = "";
            var backup = new Backup(new BackupServiceMock());
            var result = backup.GetCreationDateLastBackup();

            Assert.Equals(expected, result);
        }
    }
}
