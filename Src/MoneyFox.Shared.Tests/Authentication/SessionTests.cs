using System;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Authentication;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;

namespace MoneyFox.Shared.Tests.Authentication
{
    [TestClass]
    public class SessionTests : MvxIoCSupportingTest
    {
        [TestInitialize]
        public void Init()
        {
            ClearAll();
            Setup();
        }

        [TestMethod]
        public void ValidateSession_PasswordNotRequired_SessionValid()
        {
            var settingsSetup = new Mock<ISettings>();
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>(), false))
                .Returns(false);

            Mvx.RegisterSingleton(settingsSetup.Object);

            Assert.IsTrue(new Session().ValidateSession());
        }

        [TestMethod]
        public void ValidateSession_PasswordRequiredSessionNeverSet_SessionInvalid()
        {
            var settingsSetup = new Mock<ISettings>();
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>(), false))
                .Returns(true);

            var roamingSettingsSetup = new Mock<ISettings>();
            roamingSettingsSetup.Setup(x => x.GetValue(It.IsAny<string>(), false, true))
                .Returns(true);

            Mvx.RegisterSingleton(settingsSetup.Object);

            Assert.IsFalse(new Session().ValidateSession());
        }

        [TestMethod]
        public void ValidateSession_PasswordRequiredSession_SessionInvalid()
        {
            var settingsSetup = new Mock<ISettings>();
            settingsSetup.Setup(
                x => x.GetValue(It.Is((string s) => s == "session_timestamp"), It.IsAny<string>(), false))
                .Returns(DateTime.Now.AddMinutes(-15).ToString);
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>(), false))
                .Returns(true);

            Mvx.RegisterSingleton(settingsSetup.Object);

            new Session().ValidateSession().ShouldBeFalse();
        }

        [TestMethod]
        public void ValidateSession_PasswordRequiredSession_SessionValid()
        {
            var settingsSetup = new Mock<ISettings>();
            settingsSetup.Setup(
                x => x.GetValue(It.Is((string s) => s == "session_timestamp"), It.IsAny<string>(), false))
                .Returns(DateTime.Now.AddMinutes(-5).ToString);
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>(), false))
                .Returns(true);

            Mvx.RegisterSingleton(settingsSetup.Object);

            new Session().ValidateSession().ShouldBeTrue();
        }

        [TestMethod]
        public void AddSession_SessionTimestampAdded()
        {
            var resultDateTime = DateTime.Today.AddDays(-10);

            var settingsSetup = new Mock<ISettings>();
            settingsSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<string>(), false))
                .Callback((string key, string value, bool roam) => resultDateTime = Convert.ToDateTime(value));
            settingsSetup.Setup(x => x.GetValue(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>(), false))
                .Returns(true);

            Mvx.RegisterSingleton(settingsSetup.Object);

            new Session().AddSession();
            Assert.AreEqual(DateTime.Today, resultDateTime.Date);
        }
    }
}