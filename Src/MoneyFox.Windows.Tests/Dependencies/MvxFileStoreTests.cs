using MvvmCross.Plugins.File.Uwp;
using Xunit;

namespace MoneyFox.Windows.Tests.Dependencies
{
    public class MvxFileStoreTests
    {
        [Fact]
        public void CreateDeleteFile_FileCreatedAndDeleted()
        {
            const string filename = "TestFile";

            var fileStore = new MvxWindowsCommonFileStore();
            fileStore.WriteFile(filename, "hatschi");

            Assert.True(fileStore.Exists(filename));

            fileStore.DeleteFile(filename);

            Assert.False(fileStore.Exists(filename));
        }
    }
}
