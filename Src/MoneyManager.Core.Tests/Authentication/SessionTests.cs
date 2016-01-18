using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Test.Core;
using MoneyManager.Core.Authentication;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.TestFoundation;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Authentication
{
    public class SessionTests : MvxIoCSupportingTest
    {
        [Fact]
        public void ValidateSession_PasswordNotRequired_SessionValid()
        {
            ClearAll();
            Setup();

            var settingsSetup = new Mock<ILocalSettings>();
            settingsSetup.Setup(x => x.GetValueOrDefault(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>()))
                .Returns(false);

            Mvx.RegisterSingleton(settingsSetup.Object);

            new Session().ValidateSession().ShouldBeTrue();
        }

        [Fact]
        public void ValidateSession_PasswordRequiredSessionNeverSet_SessionInvalid()
        {
            ClearAll();
            Setup();

            var settingsSetup = new Mock<ILocalSettings>();
            settingsSetup.Setup(x => x.GetValueOrDefault(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>()))
                .Returns(true);

            var roamingSettingsSetup = new Mock<IRoamingSettings>();
            roamingSettingsSetup.Setup(x => x.GetValueOrDefault(It.IsAny<string>(), false))
                .Returns(true);

            Mvx.RegisterSingleton(settingsSetup.Object);

            new Session().ValidateSession().ShouldBeFalse();
        }

        [Theory]
        [InlineData(15, false)]
        [InlineData(5, true)]
        public void ValidateSession_PasswordRequiredSession_SessioncorrectValidated(int diffMinutes, bool expectedResult)
        {
            ClearAll();
            Setup();

            var settingsSetup = new Mock<ILocalSettings>();
            settingsSetup.Setup(
                x => x.GetValueOrDefault(It.Is((string s) => s == "session_timestamp"), It.IsAny<string>()))
                .Returns(DateTime.Now.AddMinutes(-diffMinutes).ToString);
            settingsSetup.Setup(x => x.GetValueOrDefault(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>()))
                .Returns(true);

            Mvx.RegisterSingleton(settingsSetup.Object);

            new Session().ValidateSession().ShouldBe(expectedResult);
        }

        [Fact]
        public void AddSession_SessionTimestampAdded()
        {
            ClearAll();
            Setup();

            var resultDateTime = DateTime.Today.AddDays(-10);

            var settingsSetup = new Mock<ILocalSettings>();
            settingsSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string key, string value) => resultDateTime = Convert.ToDateTime(value));
            settingsSetup.Setup(x => x.GetValueOrDefault(It.Is((string s) => s == "PasswordRequired"), It.IsAny<bool>()))
                .Returns(true);

            Mvx.RegisterSingleton(settingsSetup.Object);

            new Session().AddSession();
            resultDateTime.Date.ShouldBe(DateTime.Today);
        }
    }
}