using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MoneyManager.Business.WindowsPhone.Test.Mocks;

namespace MoneyManager.Business.WindowsPhone.Test {
    [TestClass]
    public class BackupTest {
        [TestMethod]
        public async Task Backup_GetCreationDateLastBackup() {
            const string expected = "Wednesday, October 30, 2013 12:00 AM";
            var backup = new Backup(new BackupServiceMock());
            var result = await backup.GetCreationDateLastBackup();

            Assert.AreEqual(expected, result);
        }
    }
}
