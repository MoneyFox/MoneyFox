using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Npadrutt.MvxPlugins.AppInformation.WindowsUWP;

namespace MoneyFox.Windows.Tests
{
    [TestClass]
    public class AppInformationTests
    {
        [TestMethod]
        public void GetVersion_VersionInAppManifest_CorrectVersion()
        {
            Assert.AreEqual("1.0.0.0", new WindowsAppInformation().GetVersion());
        }
    }
}