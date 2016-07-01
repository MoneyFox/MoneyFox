using Cheesebaron.MvxPlugins.Settings.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MoneyFox.Shared.Resources;
using MoneyFox.Shared.ViewModels;
using Moq;
using MvvmCross.Platform;
using MvvmCross.Test.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace MoneyFox.Shared.Tests.ViewModels {
    [TestClass]
    public class ModifyAccountViewModelTests : MvxIoCSupportingTest {

        private DateTime localDateSetting;

        [TestInitialize]
        public void Init()
        {
            ClearAll();
            Setup();

            // We setup the static setting classes here for the general usage in the app
            var settingsMockSetup = new Mock<ISettings>();
            settingsMockSetup.SetupAllProperties();
            settingsMockSetup.Setup(x => x.AddOrUpdateValue(It.IsAny<string>(), It.IsAny<DateTime>(), false))
                .Callback((string key, DateTime date, bool roam) => localDateSetting = date);

            Mvx.RegisterType(() => new Mock<IAutobackupManager>().Object);
            Mvx.RegisterType(() => settingsMockSetup.Object);
        }

        [TestMethod]
        public void Title_EditAccount_CorrectTitle() {
            var accountname = "Sparkonto";

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account {Id = 2, Name = accountname});

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object)
            {IsEdit = true};

            viewmodel.Title.ShouldBe(string.Format(Strings.EditAccountTitle, accountname));
        }

        [TestMethod]
        public void Title_AddAccount_CorrectTitle() {
            var accountname = "Sparkonto";

            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account {Id = 2, Name = accountname});

            var viewmodel = new ModifyAccountViewModel(accountRepositorySetup.Object)
            {IsEdit = false};

            viewmodel.Title.ShouldBe(Strings.AddAccountTitle);
        }

        [TestMethod]
        public void Save_UpdateTimeStamp()
        {
            var accountname = "Sparkonto";

            var accountRepoMock = new Mock<IAccountRepository>();
            var accountRepositorySetup = new Mock<IAccountRepository>();
            accountRepositorySetup.Setup(x => x.Load(It.IsAny<Expression<Func<Account, bool>>>()));
            accountRepositorySetup.SetupAllProperties();
            accountRepositorySetup.SetupGet(x => x.Selected).Returns(new Account { Id = 2, Name = accountname });
            accountRepositorySetup.Setup(x => x.AddPaymentAmount(new Payment())).Returns(true);
            accountRepositorySetup.Setup(x => x.Save(accountRepositorySetup.Object.Selected)).Returns(true);

            var accountRepo = accountRepositorySetup.Object;

            var viewmodel = new ModifyAccountViewModel(accountRepo)
            { IsEdit = false };

            viewmodel.SaveCommand.Execute();

            localDateSetting.ShouldBeGreaterThan(DateTime.Now.AddSeconds(-1));
            localDateSetting.ShouldBeLessThan(DateTime.Now.AddSeconds(1));
        }
    }
}