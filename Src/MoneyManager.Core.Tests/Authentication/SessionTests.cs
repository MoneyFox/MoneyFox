using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Test.Core;
using MoneyManager.Core.Authentication;
using MoneyManager.DataAccess;
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
            new Session(new SettingDataAccess(new Mock<IRoamingSettings>().Object) {PasswordRequired = false})
                .ValidateSession().ShouldBeTrue();
        }

        [Fact]
        public void ValidateSession_PasswordRequiredSessionNeverSet_SessionInvalid()
        {
            var roamingSettingsSetup = new Mock<IRoamingSettings>();
            roamingSettingsSetup.Setup(x => x.GetValueOrDefault(It.IsAny<string>(), false))
                .Returns(true);

            new Session(new SettingDataAccess(roamingSettingsSetup.Object))
                .ValidateSession().ShouldBeFalse();
        }

        [Theory]
        [InlineData(15, false)]
        [InlineData(5, true)]
        public void ValidateSession_PasswordRequiredSession_SessioncorrectValidated(int diffMinutes, bool expectedResult)
        {
            Setup();

            var settingsSetup = new Mock<ILocalSettings>();
            settingsSetup.Setup(x => x.GetValueOrDefault<string>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(DateTime.Now.AddMinutes(-diffMinutes).ToString);

            Mvx.RegisterSingleton(settingsSetup.Object);

            var roamingSettingsSetup = new Mock<IRoamingSettings>();
            roamingSettingsSetup.Setup(x => x.GetValueOrDefault(It.IsAny<string>(), false))
                .Returns(true);

            new Session(new SettingDataAccess(roamingSettingsSetup.Object)).ValidateSession().ShouldBe(expectedResult);
        }

        [Fact]
        public void AddSession_SessionTimestampAdded()
        {
            Setup();
            DateTime resultDateTime = DateTime.Today.AddDays(-10);

            var settingsSetup = new Mock<ILocalSettings>();
            settingsSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string key, string value) => resultDateTime = Convert.ToDateTime(value));

            Mvx.RegisterSingleton(settingsSetup.Object);

            var roamingSettingsSetup = new Mock<IRoamingSettings>();
            roamingSettingsSetup.Setup(x => x.GetValueOrDefault(It.IsAny<string>(), false))
                .Returns(true);

            new Session(new SettingDataAccess(roamingSettingsSetup.Object)).AddSession();
            resultDateTime.Date.ShouldBe(DateTime.Today);

        }
    }
}
