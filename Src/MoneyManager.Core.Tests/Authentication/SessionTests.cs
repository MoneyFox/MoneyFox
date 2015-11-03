using System;
using MoneyManager.Core.Authentication;
using MoneyManager.DataAccess;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.TestFoundation;
using Moq;
using Refractored.Xam.Settings;
using Xunit;

namespace MoneyManager.Core.Tests.Authentication
{
    public class SessionTests
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
            CrossSettings.Current.AddOrUpdateValue("password", diffMinutes);

            var roamingSettingsSetup = new Mock<IRoamingSettings>();
            roamingSettingsSetup.Setup(x => x.GetValueOrDefault(It.IsAny<string>(), false))
                .Returns(true);

            new Session(new SettingDataAccess(roamingSettingsSetup.Object)).ValidateSession().ShouldBe(expectedResult);
        }

        [Fact]
        public void AddSession_SessionTimestampAdded()
        {
            DateTime resultDateTime = DateTime.Today.AddDays(-10);
            CrossSettings.Current.AddOrUpdateValue("password", resultDateTime);

            var roamingSettingsSetup = new Mock<IRoamingSettings>();
            roamingSettingsSetup.Setup(x => x.GetValueOrDefault(It.IsAny<string>(), false))
                .Returns(true);

            new Session(new SettingDataAccess(roamingSettingsSetup.Object)).AddSession();
            resultDateTime.Date.ShouldBe(DateTime.Today);

        }
    }
}
