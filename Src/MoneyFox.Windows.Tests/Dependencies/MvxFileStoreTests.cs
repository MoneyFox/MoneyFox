using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MvvmCross.Plugins.File.WindowsCommon;

namespace MoneyFox.Windows.Tests.Dependencies
{
    [TestClass]
    public class MvxFileStoreTests
    {
        [TestMethod]
        public void CreateDeleteFile_FileCreatedAndDeleted()
        {
            const string filename = "TestFile";

            var fileStore = new MvxWindowsCommonFileStore();
            fileStore.WriteFile(filename, "hatschi");

            Assert.IsTrue(fileStore.Exists(filename));

            fileStore.DeleteFile(filename);

            Assert.IsFalse(fileStore.Exists(filename));
        }
    }
}
