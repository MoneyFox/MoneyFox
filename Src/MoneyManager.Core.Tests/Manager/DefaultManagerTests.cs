using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.Manager
{
    class DefaultManagerTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(-1, 1)]
        public void GetDefaultAccount_DefaultAccountSettings_CorrectlySelectedAccount(int defaultAccountId, int result)
        {
            var accountRepositorySetup = new Mock<IRepository<Account>>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>(
                new List<Account>
                {
                    new Account {Id = 1, CurrentBalance = 1230, Name = "Sparkonto"},
                    new Account {Id = 2, CurrentBalance = 999, Name = "Jugendkonto"},
                    new Account {Id = 3, CurrentBalance = 65, Name = "The Rest"}
                }));

            var settings = new SettingDataAccess(new Mock<IRoamingSettings>().Object) {DefaultAccount = defaultAccountId};

            var manager = new DefaultManager(accountRepositorySetup.Object, settings);

            var account = manager.GetDefaultAccount();

            account.Id.ShouldBe(result);
        }

        [Fact]
        public void GetDefaultAccount_NoSettingsNoData_CorrectFallbackValue()
        {
            var accountRepositorySetup = new Mock<IRepository<Account>>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account {Id = 5});

            var settings = new SettingDataAccess(new Mock<IRoamingSettings>().Object) { DefaultAccount = -1 };

            var manager = new DefaultManager(accountRepositorySetup.Object, settings);

            var account = manager.GetDefaultAccount();

            account.Id.ShouldBe(5);
        }

        [Fact]
        public void GetDefaultAccount_NoSettingsNoDataNoSelected_CorrectFallbackValue()
        {
            var accountRepositorySetup = new Mock<IRepository<Account>>();
            accountRepositorySetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var settings = new SettingDataAccess(new Mock<IRoamingSettings>().Object) { DefaultAccount = -1 };

            var manager = new DefaultManager(accountRepositorySetup.Object, settings);

            var account = manager.GetDefaultAccount();

            account.ShouldBeNull();
        }
    }
}
