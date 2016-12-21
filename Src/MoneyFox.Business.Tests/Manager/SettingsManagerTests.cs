using System;
using System.Dynamic;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MoneyFox.Business.Manager;
using Moq;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.Business.Tests.Manager
{
    public class SettingsManagerTests
    {
        [Fact]
        public void DefaultAccount_DefaultValue()
        {
            int result = 0;

            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<int>(), true)).Callback(
                (string key, int value, bool shallSync) =>
                {
                    result = 0;
                });

            new SettingsManager(settingsMockSetup.Object).DefaultAccount.ShouldBe(-1);
        }
    }
}
