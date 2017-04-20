using Xunit;
using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace MoneyFox.Windows.Tests
{
    public class AppInformationTests
    {
        [Fact]
        public void GetVersion_VersionInAppManifest_CorrectVersion()
        {
            Assert.AreEqual("1.0.0.0", new WindowsAppInformation().GetVersion());
        }
    }
}