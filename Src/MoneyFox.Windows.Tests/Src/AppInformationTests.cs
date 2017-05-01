using Xunit;

namespace MoneyFox.Windows.Tests
{
    public class AppInformationTests
    {
        [Fact]
        public void GetVersion_VersionInAppManifest_CorrectVersion()
        {
            Assert.Equal("2.1.0.0", new WindowsAppInformation().GetVersion());
        }
    }
}