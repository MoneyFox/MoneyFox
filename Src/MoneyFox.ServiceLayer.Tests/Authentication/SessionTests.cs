using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.BusinessLogic.Adapters;
using MoneyFox.ServiceLayer.Authentication;
using MoneyFox.ServiceLayer.Facades;
using Moq;
using Should;
using Xunit;

namespace MoneyFox.ServiceLayer.Tests.Authentication
{
    [ExcludeFromCodeCoverage]
    [Collection("MvxIocCollection")]
    public class SessionTests
    {
        [Fact]
        public void ValidateSession_PasswordNotRequired_SessionValid()
        {
            Assert.True(new Session(new Mock<ISettingsFacade>().Object).ValidateSession());
        }

        [Fact]
        public void ValidateSession_PasswordRequiredSessionNeverSet_SessionInvalid()
        {
            var settingsSetup = new Mock<ISettingsAdapter>();
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>()))
                .Returns(true);

            Assert.False(new Session(new SettingsFacade(settingsSetup.Object)).ValidateSession());
        }

        [Fact]
        public void ValidateSession_PasswordRequiredSession_SessionInvalid()
        {
            var settingsSetup = new Mock<ISettingsAdapter>();
            settingsSetup.Setup(
                x => x.GetValue(It.Is((string s) => s == "session_timestamp"), It.IsAny<string>()))
                .Returns(DateTime.Now.AddMinutes(-15).ToString);
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>()))
                .Returns(true);

            new Session(new SettingsFacade(settingsSetup.Object)).ValidateSession().ShouldBeFalse();
        }

        [Fact]
        public void ValidateSession_PasswordRequiredSession_SessionValid()
        {
            var settingsSetup = new Mock<ISettingsAdapter>();
            settingsSetup.Setup(
                x => x.GetValue(It.Is((string s) => s == "session_timestamp"), It.IsAny<string>()))
                .Returns(DateTime.Now.AddMinutes(-5).ToString);
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>()))
                .Returns(true);

            new Session(new SettingsFacade(settingsSetup.Object)).ValidateSession().ShouldBeTrue();
        }

        [Fact]
        public void AddSession_SessionTimestampAdded()
        {
            var resultDateTime = DateTime.Today.AddDays(-10);

            var settingsSetup = new Mock<ISettingsAdapter>();
            settingsSetup.Setup(x => x.AddOrUpdate(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string key, string value) => resultDateTime = Convert.ToDateTime(value));
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>()))
                .Returns(true);

            new Session(new SettingsFacade(settingsSetup.Object)).AddSession();
            Assert.Equal(DateTime.Today, resultDateTime.Date);
        }
    }
}