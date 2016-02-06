using NUnit.Framework;

namespace MoneyManager.Windows.Tests
{
    [TestFixture]
    public class AppInformationTests
    {
        [Test]
        public void GetVersion_VersionInAppManifest_CorrectVersion()
        {
            Assert.AreEqual("1.0.0.0", new AppInformation().Version);
        }
    }
}