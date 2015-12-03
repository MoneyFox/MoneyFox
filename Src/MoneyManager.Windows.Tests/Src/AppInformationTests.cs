using MoneyManager.TestFoundation;
using MoneyManager.Windows.Concrete;

using Xunit;

namespace MoneyManager.Windows.Tests
{
    public class AppInformationTests
    {
        [Fact]
        public void GetVersion_VersionInAppManifest_CorrectVersion()
        {
            new AppInformation().GetVersion.ShouldBe("1.0.0.0");
        }
    }
}