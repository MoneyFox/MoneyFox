using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoneyManager.Core.Manager;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Manager
{
    public class DefaultManagerTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(-1, 1)]
        public void GetDefaultAccount_DefaultAccountSettings_CorrectlySelectedAccount(int defaultAccountId, int result)
        {
            //Setup
            var accountRepositorySetup = new Mock<IRepository<Account>>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>(
                new List<Account>
                {
                    new Account {Id = 1, CurrentBalance = 1230, Name = "Sparkonto"},
                    new Account {Id = 2, CurrentBalance = 999, Name = "Jugendkonto"},
                    new Account {Id = 3, CurrentBalance = 65, Name = "The Rest"}
                }));

            var roamingSettingsSetup = new Mock<IRoamingSettings>();
            roamingSettingsSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<object>()));
            roamingSettingsSetup.Setup(x => x.GetValueOrDefault(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(defaultAccountId);

            //Execute
            var account =
                new DefaultManager(accountRepositorySetup.Object, new SettingDataAccess(roamingSettingsSetup.Object))
                    .GetDefaultAccount();

            //Assert
            account.Id.ShouldBe(result);
        }

        [Fact]
        public void GetDefaultAccount_SelectedSettingsData_CorrectFallbackValue()
        {
            //Setup
            var accountRepositorySetup = new Mock<IRepository<Account>>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>(
                new List<Account>
                {
                    new Account {Id = 1, CurrentBalance = 1230, Name = "Sparkonto"},
                    new Account {Id = 2, CurrentBalance = 999, Name = "Jugendkonto"},
                    new Account {Id = 3, CurrentBalance = 65, Name = "The Rest"}
                }));
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account {Id = 2});

            var roamingSettingsSetup = new Mock<IRoamingSettings>();
            roamingSettingsSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<object>()));
            roamingSettingsSetup.Setup(x => x.GetValueOrDefault(It.IsAny<string>(), It.IsAny<int>())).Returns(3);

            //Execute
            var account = new DefaultManager(accountRepositorySetup.Object,
                new SettingDataAccess(roamingSettingsSetup.Object))
                .GetDefaultAccount();

            //Assert
            account.Id.ShouldBe(2);
        }

        [Fact]
        public void GetDefaultAccount_NoSettingsNoData_CorrectFallbackValue()
        {
            //Setup
            var accountRepositorySetup = new Mock<IRepository<Account>>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account {Id = 5});

            //Execute
            var account = new DefaultManager(accountRepositorySetup.Object,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object) {DefaultAccount = -1})
                .GetDefaultAccount();

            //Assert
            account.Id.ShouldBe(5);
        }

        [Fact]
        public void GetDefaultAccount_SelectedAndDataNoSettings_CorrectFallbackValue()
        {
            //Setup
            var accountRepositorySetup = new Mock<IRepository<Account>>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>(
                new List<Account>
                {
                    new Account {Id = 1, CurrentBalance = 1230, Name = "Sparkonto"},
                    new Account {Id = 2, CurrentBalance = 999, Name = "Jugendkonto"},
                    new Account {Id = 3, CurrentBalance = 65, Name = "The Rest"}
                }));
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account {Id = 2});

            //Execute
            var account = new DefaultManager(accountRepositorySetup.Object,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object) {DefaultAccount = -1})
                .GetDefaultAccount();

            //Assert
            account.Id.ShouldBe(2);
        }

        [Fact]
        public void GetDefaultAccount_NoSettingsNoDataNoSelected_CorrectFallbackValue()
        {
            //Setup
            var accountRepositorySetup = new Mock<IRepository<Account>>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            //Execute
            var manager = new DefaultManager(accountRepositorySetup.Object,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object) {DefaultAccount = -1});

            var account = manager.GetDefaultAccount();

            //Assert
            account.ShouldBeNull();
        }
    }
}