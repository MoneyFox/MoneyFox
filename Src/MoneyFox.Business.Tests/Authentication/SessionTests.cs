using System;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MoneyFox.Business.Authentication;
using MoneyFox.Business.Manager;
using MoneyFox.Foundation.Interfaces;
using Moq;
using Xunit;

namespace MoneyFox.Business.Tests.Authentication
{
    [Collection("MvxIocCollection")]
    public class SessionTests
    {
        [Fact]
        public void ValidateSession_PasswordNotRequired_SessionValid()
        {
            Assert.True(new Session(new Mock<ISettingsManager>().Object).ValidateSession());
        }

        [Fact]
        public void ValidateSession_PasswordRequiredSessionNeverSet_SessionInvalid()
        {
            var settingsSetup = new Mock<ISettings>();
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>(), false))
                .Returns(true);

            Assert.False(new Session(new SettingsManager(settingsSetup.Object)).ValidateSession());
        }

        [Fact]
        public void ValidateSession_PasswordRequiredSession_SessionInvalid()
        {
            var settingsSetup = new Mock<ISettings>();
            settingsSetup.Setup(
                x => x.GetValue(It.Is((string s) => s == "session_timestamp"), It.IsAny<string>(), false))
                .Returns(DateTime.Now.AddMinutes(-15).ToString);
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>(), false))
                .Returns(true);

            new Session(new SettingsManager(settingsSetup.Object)).ValidateSession().ShouldBeFalse();
        }

        [Fact]
        public void ValidateSession_PasswordRequiredSession_SessionValid()
        {
            var settingsSetup = new Mock<ISettings>();
            settingsSetup.Setup(
                x => x.GetValue(It.Is((string s) => s == "session_timestamp"), It.IsAny<string>(), false))
                .Returns(DateTime.Now.AddMinutes(-5).ToString);
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>(), false))
                .Returns(true);

            new Session(new SettingsManager(settingsSetup.Object)).ValidateSession().ShouldBeTrue();
        }

        [Fact]
        public void AddSession_SessionTimestampAdded()
        {
            var resultDateTime = DateTime.Today.AddDays(-10);

            var settingsSetup = new Mock<ISettings>();
            settingsSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<string>(), false))
                .Callback((string key, string value, bool roam) => resultDateTime = Convert.ToDateTime(value));
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>(), false))
                .Returns(true);

            new Session(new SettingsManager(settingsSetup.Object)).AddSession();
            Assert.Equal(DateTime.Today, resultDateTime.Date);
        }
    }
}